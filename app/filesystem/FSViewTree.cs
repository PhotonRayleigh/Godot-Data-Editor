using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

public class FSViewTree
{
    /*
        Scratch pad:
        First things first I need to get down the process that will formulate the
        application's internal cache of the file system in question. It will be a simple
        tree that represents all the directories the user is currently browsing. 
        
        I should also make a list to store directories that are opened by the user.
        Thinking:
        DirNode UserRoot;
        List<DirNode> OpenedDirs = new List<DirNode>();

        Should I include a field to indicate if a node is open or not? Probably.
        I should be able to use that to determine how deep to crawl the directory.

        Perhaps... I should handle the trees in a disposable way. Every refresh, build a new tree using the
        old tree, then discard the old tree? I do need opened objects to be presistant though....
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

    DirNode userRootDir;
    List<DirNode> userOpenDirs = new List<DirNode>();

    public FSViewTree()
    {
        userDataDir = OS.GetUserDataDir();
        exePath = OS.GetExecutablePath();
        //userRootDir = new DirNode(userDataDir); // user:// is the default path.
        SetRootDirectory(userDataDir);
        isRootDirUser = true;
    }

    public void SetRootDirectory(string path)
    {
        GD.Print("FSViewTree.cs: Setting Root directory")
        // Need to check if directory exists or not.
        userRootDir = new DirNode(path);
        userRootDir.isOpen = true;
        // Crawl the root directory
        new task(() => ScanDirectory(userRootDir));
        GD.Print("FSViewTree.cs: Exiting SetRootDirectory()")
    }

    public void ScanDirectory(DirNode scanNode)
    {
        // Iterate folders
        Task iterateFolders = new Task(() =>
        {
            int listCount = scanNode.folders.Count;
            for (int i = 0; i < listCount; i++)
            {
                if (scanNode.folders[i].thisDir.Exists)
                {
                    continue;
                }
                else
                {
                    scanNode.folders.RemoveAt(i);
                }
            }
        });

        // Iterate files
        Task iterateFiles = new Task(() =>
        {
            int listCount = scanNode.files.Count;
            for (int i = 0; i < listCount; i++)
            {
                if (scanNode.files[i].thisFile.Exists)
                {
                    continue;
                }
                else
                {
                    scanNode.files.RemoveAt(i);
                }
            }
        });



        DirectoryInfo[] directories = scanNode.thisDir.GetDirectories();
        FileInfo[] files = scanNode.thisDir.GetFiles();
        iterateFolders.Wait();
        iterateFiles.Wait();

        // Check for new folders and files
        iterateFolders = new Task(() =>
        {
            int listCount = directories.GetLength(0);
            for (int i = 0; i < listCount; i++)
            {
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
        });

        iterateFiles = new Task(() =>
        {
            int listCount = files.GetLength(0);
            for (int i = 0; i < listCount; i++)
            {
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
        });

        iterateFolders.Wait();
        iterateFiles.Wait();
        return;
    }

    public void OpenDirectory(DirNode openNode)
    {
        openNode.isOpen = true;
        ScanDirectory(openNode);
    }

    public void CloseDirectory(DirNode closeNode)
    {
        closeNode.isOpen = false;
        foreach (DirNode subNode in closeNode.folders)
        {
            subNode.folders.Clear();
            subNode.files.Clear();
        }
    }

    public void RefreshDirectories()
    // TODO: Hook this up and test it.
    {
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

        int level = 0;
        int nodes = 0;
        bool scanning = true;
        ScanDirectory(userRootDir);
        Stack<IterationInfo> itrStack = new Stack<IterationInfo>();
        IterationInfo currentItr = new IterationInfo() { Count = userRootDir.folders.Count };
        DirNode currentDir = userRootDir;

        while (scanning)
        {
            // We are at top
            // Scan each directory
            // if a directory has sub directories, push state on stack and scan sub directories
            // when done with a sub directory, pop the stack and return to the parent iterator
            GD.Print($"Current directory is: {currentDir.path} on iteration {currentItr.currentIndex} out of {currentItr.FinalIndex}");
            if (currentItr.Count > 0)
            {
                ScanDirectory(currentDir.folders[currentItr.currentIndex]);
                if (currentDir.folders[currentItr.currentIndex].folders.Count > 0)
                {
                    currentDir = currentDir.folders[currentItr.currentIndex];
                    itrStack.Push(currentItr);
                    currentItr = new IterationInfo() { Count = currentDir.folders.Count };

                    continue; // We are starting fresh, to take it back to the top.
                              // Only increment and check when we don't step into 
                              // a new folder
                }
            }

            currentItr.currentIndex++;
            if (currentItr.currentIndex > currentItr.FinalIndex && currentDir.parent != null)
            {
                currentItr = itrStack.Pop();

                currentDir = currentDir.parent;
            }
            else if (currentItr.currentIndex > currentItr.FinalIndex && currentDir.parent == null)
            {
                scanning = false;
            }
        }
    }

    // Custom Types for this class 
    public enum NodeType { folder, file }

    public abstract class Node
    {
        public readonly NodeType type;
        public DirNode? parent;
        public string path;
        public string name;
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
}