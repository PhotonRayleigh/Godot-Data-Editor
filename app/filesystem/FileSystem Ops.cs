using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SparkLib;

public partial class FileSystem : Panel
{
    // handling file operations here

    public void Copy(FSViewTree.DirNode source, FSViewTree.DirNode dest)
    {
        if (source.thisDir.Exists)
        {
            // Perform recursive copy:
            // Check new directory if the directory already exists
            // if it does, increment name until no conflict
            // Create new directory
            // Recursively copy files and folders from old directory to new
        }
    }

    public void Copy(FSViewTree.FileNode source, FSViewTree.DirNode dest)
    {
        // Copy source file into destination directory
        // If file name exists, increment until no conflict.
    }

    public void HardDelete(FSViewTree.DirNode target)
    {
        // TODO: If the directory is not empty,
        // warn the user and ask for confirmation.

        // TODO: Implement a caching system (either custom
        // or maybe recycle bin) to allow the user to undo file
        // operations.
        // After a little research, it appears making a cross platform
        // delete that uses the system recycling bin might be tricky.
        // On Windows, at least, it requires non-portable code.
        // I can implement a custom temp one first then see about using
        // system ones later, but that will have to change based on the
        // target platform at build time.

        // Access DirectoryInfo
        // Call detele function
        // update FSviewTree to reflect changes (remove node)
        // Update FileSystemList

        // Deletes recursively, this is a hard delete and cannot be undone?
        if (target.thisDir.Exists) target.thisDir.Delete(true);
        return;
    }

    public void HardDelete(FSViewTree.FileNode target)
    {
        // Perminant deletion, I want to make one that 
        if (target.thisFile.Exists) target.thisFile.Delete();
        return;
    }

    public void SoftDelete()
    {
        GD.PrintErr("Function FileSystem.SoftDelete called, which is not implemented yet")
    }

    public void Move(FSViewTree.DirNode source, FSViewTree.DirNode dest)
    {
        if (source.thisDir.Exists) source.thisDir.MoveTo($"{dest.path}/{source.name}");
        return;
    }

    public void Move(FSViewTree.FileNode source, FSViewTree.DirNode dest)
    {
        if (source.thisFile.Exists) source.thisFile.MoveTo($"{dest.path}/{source.name}");
        return;
    }

    public void Move(List<FSViewTree.Node> source, FSViewTree.DirNode dest)
    {
        if (source.Count == 0) return;
        foreach (FSViewTree.Node item in source)
        {
            if (item is FSViewTree.DirNode)
            {
                Move((item as FSViewTree.DirNode)!, dest);
            }
            else if (item is FSViewTree.FileNode)
            {
                Move((item as FSViewTree.FileNode)!, dest);
            }
        }
        return;
    }

    public void Move(Godot.Collections.Array<FSViewTree.Node> source, FSViewTree.DirNode dest)
    {
        if (source.Count == 0) return;
        foreach (FSViewTree.Node item in source)
        {
            if (item is FSViewTree.DirNode)
            {
                // Access item.thisDir to move it
                FSViewTree.DirNode temp = (item as FSViewTree.DirNode)!;
                temp.thisDir.MoveTo($"{dest.path}/{temp.name}");
            }
            else if (item is FSViewTree.FileNode)
            {
                // Access item.thisFile to move it
                FSViewTree.FileNode temp = (item as FSViewTree.FileNode)!;
                temp.thisFile.MoveTo($"{dest.path}/{temp.name}");
            }
        }
        RefreshFileSystem();
        return;
    }

    public void Rename(FSViewTree.FileNode target, string name)
    {

    }

    public void Rename(FSViewTree.DirNode target, string name)
    {

    }

    public void CreateFile(FSViewTree.DirNode location, string name)
    {

    }

    public void CreateFolder(FSViewTree.DirNode location, string name)
    {

    }
}