using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

public class FSViewTree
{
    /*
        Scratch pad:
        New paradigm: thread friendly, not threaded by default.
        I would like all of the FSViewTree operations to happen in their own thread.
        However, I don't know how to properly do this without making a terrible mess.

        So, instead, I will make the class thread friendly, and if the calling objects need
        the functions to happen in threads, the calling object can handle that.
    */

    /*
        FSViewTree:
        Godot doesn't include conversion of paths, it only uses forward slash notation.
        i.e.
        res://textures/image.png
        user://saves/mysave.json
        C:/users/NO3fu/Documents/myText.txt

        Windows supports forward slashes in paths, but it might not return paths with forward slashes.
        So if you need to get a directory returned by an OS function outside of Godot, just make sure to
        convert the backslashes to forward. 
        
        So, the special cases I need to handle are the user:// and res:// directories, which won't be intelligable
        to functions outside of Godot. So I just need to store their system paths, which is contained in Godot's OS object.
        There is also a Cache directory that might be useful to remember as well.

        Key locations:
        OS.GetExecutablePath() - gets the location of the currently executed engine binary.
        OS.GetSystemDir(OS.SystemDir dir) - returns the location of common host OS locations (like Desktop or Documents).
        OS.GetUserDataDir() - returns the system path for the user data directory.

        I don't think I can get the location of res:// from Godot, but the resources should be bundled with Godot's executable anyway,
        meaning GetExecutablePath() should be sufficient if I need to get at it for some reason. Or it can be something I just need to
        ask the user for. Regardless, once shipped, res:// is read only anyway. So if I make a program that requires writing
        to it's install directory (like Steam does) then I will just need to handle that in a custom way. I am not worried about it.

        Working directory - this is something I need to implement and handle myself.
    */
    const string GDUserPath = "user://";
    const string GDResPath = "res://";

    bool isRootDirUser = false; // I want to use the user:// moniker when the user uses the user directory.
    // I am not sure the best way to do that.
    // When using anything else, I want the full path displayed.
    // Maybe the thing to do is have a selector in the UI that let's the user switch modes:
    // System based paths or user based paths. Also have a button to display the full path to the user:// directory.
    // Or make the user just select a working path on their own. Or have an option for each. idk. 
    readonly string userDataDir = "";
    readonly string exePath = "";

    public DirNode? userRootDir;
    public List<DirNode> userOpenDirs = new List<DirNode>();

    public FSViewTree()
    {
        userDataDir = OS.GetUserDataDir();
        exePath = OS.GetExecutablePath();
        SetRootDirectory(userDataDir);
    }

    public FSViewTree(string rootPath)
    {
        userDataDir = OS.GetUserDataDir();
        exePath = OS.GetExecutablePath();
        SetRootDirectory(rootPath);
    }

    public void SetRootDirectory(string path)
    {
        string cleanPath = path.Replace('\\', '/');

        if (cleanPath.Equals(GDUserPath))
        {
            cleanPath = userDataDir;
            isRootDirUser = true;
        }
        else if (cleanPath.Equals(userDataDir)) isRootDirUser = true;
        else isRootDirUser = false;

        userRootDir = new DirNode(cleanPath);

        if (userRootDir.thisDir.Exists == false)
        {
            throw new FileNotFoundException($"Error: FSViewTree.SetRootDirectory() attempted to open path \'{path}\', which does not exist.");
        }

        userRootDir.isOpen = true;

        // Crawl the root directory
        ScanDirectory(userRootDir);
    }


    public void ScanDirectory(DirNode scanNode)
    {
        // Check that existing folders still exist
        Task iterateFolders = Task.Run(() =>
        {
            //GD.Print("Entering iterateFolders 1");
            int listCount = scanNode.folders.Count;
            for (int i = 0; i < listCount; i++)
            {
                //GD.Print($"iterateFolders i = {i}");
                if (scanNode.folders[i].thisDir.Exists)
                {
                    continue;
                }
                else
                {
                    scanNode.folders.RemoveAt(i);
                }
            }
            //GD.Print("Exiting iterateFolders 1");
        });

        // Check that existing files still exist
        Task iterateFiles = Task.Run(() =>
        {
            //GD.Print("Entering iterateFiles 1");
            int listCount = scanNode.files.Count;
            for (int i = 0; i < listCount; i++)
            {
                //GD.Print($"iterateFiles i = {i}");
                if (scanNode.files[i].thisFile.Exists)
                {
                    continue;
                }
                else
                {
                    scanNode.files.RemoveAt(i);
                }
            }
            //GD.Print("Exiting iterateFiles 1");
        });

        DirectoryInfo[] directories = scanNode.thisDir.GetDirectories();
        FileInfo[] files = scanNode.thisDir.GetFiles();
        iterateFolders.Wait();
        iterateFiles.Wait();

        // Check for new folders
        iterateFolders = Task.Run(() =>
        {
            //GD.Print("Entering iterateFolders 2");
            int listCount = directories.GetLength(0);
            for (int i = 0; i < listCount; i++)
            {
                //GD.Print($"iterateFolders i = {i}");
                if (scanNode.folders.Exists((DirNode d) =>
                {
                    if (d.thisDir.FullName.Equals(directories[i].FullName)) return true;
                    else return false;
                })) continue;
                else
                {
                    scanNode.folders.Add(new DirNode(directories[i], scanNode));
                }
            }
            //GD.Print("Exiting iterateFolders 2");
        });

        // Check for new files
        iterateFiles = Task.Run(() =>
        {
            //GD.Print("Entering iterateFiles 2");
            int listCount = files.GetLength(0);
            for (int i = 0; i < listCount; i++)
            {
                //GD.Print($"iterateFiles i = {i}");
                if (scanNode.files.Exists((FileNode f) =>
                {
                    if (f.thisFile.FullName.Equals(files[i].FullName)) return true;
                    else return false;
                })) continue;
                else
                {
                    scanNode.files.Add(new FileNode(files[i], scanNode));
                }
            }
            //GD.Print("Exiting iterateFiles 2");
        });

        iterateFolders.Wait();
        iterateFiles.Wait();
        return;
    }

    public void OpenDirectory(DirNode openNode)
    {
        openNode.isOpen = true;
        ScanDirectory(openNode);
        return;
    }

    public void CloseDirectory(DirNode closeNode)
    {
        closeNode.isOpen = false;
        foreach (DirNode subNode in closeNode.folders)
        {
            subNode.folders.Clear();
            subNode.files.Clear();
        }
        return;
    }

    public void PrintRoot()
    {
        foreach (DirNode directory in userRootDir!.folders)
        {
            GD.Print($"RootDir entry: {directory.path}");
        }
        foreach (FileNode File in userRootDir.files)
        {
            GD.Print($"RootDir entry: {File.path}");
        }
    }

    public void PrintTree(DirNode directory)
    {
        foreach (DirNode folder in directory.folders)
        {
            GD.Print(folder.path);
            if (folder.folders.Count > 0)
            {
                PrintTree(folder);
            }
        }
        foreach (FileNode file in directory.files)
        {
            GD.Print(file.path);
        }
    }


    protected bool isRefreshing = false;
    public bool IsRefreshing { get => isRefreshing; }

    // While refreshing is happening, do not refresh again within the same FSViewTree.
    public void RefreshDirectories()
    {
        if (isRefreshing) return;
        isRefreshing = true;
        //GD.Print("RefreshDirectories(): Entering function");
        if (userRootDir == null)
        {
            GD.PrintErr("FSViewTree.RefreshDirectories(): Error, root directory is not set.");
            return;
        }
        if (!userRootDir.thisDir.Exists)
        {
            GD.PrintErr("FSViewTree.RefreshDirectories(): Error, root directory does not exist.");
            return;
        }

        bool scanning = true;
        ScanDirectory(userRootDir);
        Stack<IterationInfo> itrStack = new Stack<IterationInfo>();
        IterationInfo currentItr = new IterationInfo() { Count = userRootDir.folders.Count };
        DirNode currentDir = userRootDir;

        while (scanning)
        {
            //GD.Print($"Current directory is: {currentDir.path} on iteration {currentItr.currentIndex} out of {currentItr.FinalIndex}");
            if ((currentItr.currentIndex < currentItr.Count) && currentDir.isOpen)
            {
                DirNode checkFolder = currentDir.folders[currentItr.currentIndex];
                ScanDirectory(checkFolder);
                if (checkFolder.folders.Count > 0)
                {
                    //currentItr.currentIndex++;
                    itrStack.Push(currentItr);
                    currentDir = checkFolder;
                    currentItr = new IterationInfo() { Count = currentDir.folders.Count };

                    continue;
                }
            }

            if (currentItr.currentIndex >= currentItr.FinalIndex && currentDir.parent != null)
            {
                currentItr = itrStack.Pop();

                currentDir = currentDir.parent;
            }
            else if (currentItr.currentIndex >= currentItr.FinalIndex && currentDir.parent == null)
            {
                scanning = false;
            }

            currentItr.currentIndex++;
        }
        //GD.Print("RefreshDirectories(): Exiting function");
        isRefreshing = false;
        return;
    }

    // Custom Types for this class 
    public enum NodeType { folder, file }

    public abstract class Node
    {
        public readonly NodeType type;
        public DirNode? parent;
        public string? path;
        public string? name;
        public bool isOpen = false;


        public Node(FSViewTree.NodeType type)
        {
            this.type = type;
        }
    }

    public class DirNode : FSViewTree.Node
    {
        public DirectoryInfo thisDir;
        public List<DirNode> folders = new List<DirNode>();
        public List<FileNode> files = new List<FileNode>();

        internal DirNode(string path, DirNode? parent = null) : base(FSViewTree.NodeType.folder)
        {
            this.path = path;
            thisDir = new DirectoryInfo(path);
            this.name = thisDir.Name;
            this.parent = parent; // null if root or orphon. 
        }

        internal DirNode(DirectoryInfo dir, DirNode? parent = null) : base(FSViewTree.NodeType.folder)
        {
            thisDir = dir;
            this.path = thisDir.ToString();
            this.name = thisDir.Name;
            this.parent = parent;
        }
    }

    public class FileNode : FSViewTree.Node
    {
        public FileInfo thisFile;

        internal FileNode(string path, DirNode? parent = null) : base(FSViewTree.NodeType.file)
        {
            this.path = path;
            thisFile = new FileInfo(path);
            this.name = thisFile.Name;
            this.parent = parent; // null if root or orphon. 
        }

        internal FileNode(FileInfo file, DirNode? parent = null) : base(FSViewTree.NodeType.file)
        {
            thisFile = file;
            this.path = thisFile.ToString();
            this.name = thisFile.Name;
            this.parent = parent;
        }
    }

    protected class IterationInfo
    {
        public const int startIndex = 0;
        public int currentIndex = 0;
        private int finalIndex = 0;
        private int count = 0;

        public int FinalIndex
        {
            get => finalIndex;
            set
            {
                finalIndex = value;
                count = value + 1;
            }
        }
        public int Count
        {
            get => count;
            set
            {
                count = value;
                finalIndex = value - 1;
            }
        }
    }

    protected class DirectoryNotFoundException : Exception
    {
        public DirectoryNotFoundException()
        {

        }
        public DirectoryNotFoundException(string message) : base(message)
        {

        }

        public DirectoryNotFoundException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}