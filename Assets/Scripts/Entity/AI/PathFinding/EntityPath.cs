using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
[System.Serializable]
public class EntityPath
{

    public Vec2i Target;
    public List<Vec2i> Path;
    private int index;
    public EntityPath(List<Vec2i> path)
    {
        Path = path;
        index = 0;
        Debug.Log(path.Count);
        if(path.Count != 0)
            Target = path[Path.Count - 1];
    }

    public Vec2i CurrentIndex()
    {
        return Path[index];
    }

    public Vec2i NextIndex()
    {
        index++;
        if (index == Path.Count)
            return null;
        return Path[index];
    }

    /// <summary>
    /// Called when the end target of out path has changed.
    /// We iterate through the points in our path, and check the distance to 
    /// the new target. If the distance is lower, we re-calculate the path
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public bool UpdateTarget(Vec2i target)
    {
        Vec2i closestPoint = Path[index];
        int closestDist = Vec2i.QuickDistance(target, closestPoint);
        int closestPointIndex = index;
        for(int i=index; i<Path.Count; i+=2)
        {
            int thisDist = Vec2i.QuickDistance(target, Path[i]) + (i-index);
            if(thisDist < closestDist)
            {
                closestDist = thisDist;
                closestPoint = Path[i];
                closestPointIndex = i;
            }
        }
        if(closestPointIndex + 1 < Path.Count)
        {
            int trimAmount = Path.Count - 1 - (closestPointIndex + 1);
            if(trimAmount > 0)
            {
                //Remove end of path 
                Path.RemoveRange(closestPointIndex + 1, Path.Count - 1 - (closestPointIndex + 1));
            }
           
        }
        if(index > 1)
        {
            Path.RemoveRange(0, index - 1);
            index = 0;
        }
        




        List<Vec2i> extension = GameManager.PathFinder.GeneratePath(closestPoint, target);
        if (extension.Count == 0)
            return false;


        Path.AddRange(extension);

        Target = target;

        return true;

    }

}