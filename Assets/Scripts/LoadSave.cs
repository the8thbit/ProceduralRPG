using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading;
public class LoadSave
{
    public static string CHUNK_REGION_SAVE = "/ChunkRegions/";
    private static string PersistentDataPath;
    public string SaveFile { get; private set; }
    public LoadSave(string saveFile)
    {
        PersistentDataPath = Application.persistentDataPath;
        SaveFile = saveFile;
    }

    public void SaveChunkRegion(ChunkRegion cr)
    {
        string file = PersistentDataPath + "/" + SaveFile + "/ChunkRegions/" + (cr.X + "_" + cr.Z) + ".save";
        string direction = PersistentDataPath + "/" + SaveFile + "/ChunkRegions";
        Directory.CreateDirectory(direction);
        //File.Cre

        //UnityEngine.Profiling.Profiler.BeginSample("serialise");
        Debug.Log("Saving region " + cr.X + "_" + cr.Z, Debug.WORLD_GENERATION);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Create(file);
        bf.Serialize(new BufferedStream(fs), cr);
        fs.Close();

        //UnityEngine.Profiling.Profiler.EndSample();


    }
    public ChunkRegion LoadChunkRegion(int x, int z)
    {
        string filePath = PersistentDataPath + "/" + SaveFile + "/ChunkRegions/" + (x + "_" + z) + ".save";
        if (File.Exists(filePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open);
            Debug.Log("Loading region " + x + "_" + z, Debug.CHUNK_LOADING);
            ChunkRegion r = (ChunkRegion)bf.Deserialize(file);
            file.Close();
            return r;


        }
        return null;
    }

    public void Save(GameLoadSave gls)
    {
        string file = PersistentDataPath + "/" + SaveFile + "/World.save";
        string kingdomFile = PersistentDataPath + "/" + SaveFile + "/WorldKingdoms.save";
        string settlementFile = PersistentDataPath + "/" + SaveFile + "/WorldSettlements.save";
        string entityFile = PersistentDataPath + "/" + SaveFile + "/Entities1.save";
        string entityFile2 = PersistentDataPath + "/" + SaveFile + "/Entities2.save";

        string direction = PersistentDataPath + "/" + SaveFile + "/ChunkRegions";
        string setDir = PersistentDataPath + "/" + SaveFile + "/WorldSettlements";
        Directory.CreateDirectory(direction);
        Directory.CreateDirectory(setDir);

        foreach(ChunkRegion r in gls.ChunkRegions)
        {
            if(r!=null)
                SaveChunkRegion(r);

        }
        
        gls.GatherData();

        List<Thread> threads = new List<Thread>();


        threads.AddRange(InternalSettlementSave(gls));

        Thread kingThread = new Thread(() => ThreadSaveKingdoms(gls));
        kingThread.Start();
        threads.Add(kingThread);

        /*
        BinaryFormatter bf = new BinaryFormatter();
        FileStream king = File.Create(kingdomFile);
        bf.Serialize(king, gls.WorldKingdoms);
        king.Close();*/
        Thread setThread = new Thread(() => ThreadSaveEntity(gls));
        setThread.Start();
        threads.Add(setThread);


        bool isFin = false;
        while (!isFin)
        {
            isFin = true;
            foreach(Thread t in threads)
            {
                if (t.IsAlive)
                    isFin = false;
            }
        }
        /*
        FileStream set = File.Create(settlementFile);
        bf.Serialize(set, gls.WorldSettlements);
        set.Close();

        FileStream ent1 = File.Create(entityFile);
        bf.Serialize(ent1, gls.GameEntities);
        ent1.Close();

        FileStream ent2 = File.Create(entityFile2);
        bf.Serialize(ent2, gls.GameEntityChunks);
        ent2.Close();
        //FileStream fs = File.Create(file);
        //bf.Serialize(fs, gls);
        //fs.Close();*/
    }
    private List<Thread> InternalSettlementSave(GameLoadSave gls)
    {
        List<Thread> setSaveThreads = new List<Thread>();
        Settlement[] toSave = new Settlement[5];
        int sCount = 0;
        foreach(KeyValuePair<int,Settlement> kvp in gls.WorldSettlements)
        {
            toSave[sCount] = kvp.Value;
            sCount++;
            if (sCount == 5)
            {
                sCount = 0;
                Thread setSave = new Thread(() => ThreadSaveSettlements(toSave));
                setSave.Start();
                setSaveThreads.Add(setSave);
                toSave = new Settlement[5];
            }
        }
        //Save remaining
        Thread last = new Thread(() => ThreadSaveSettlements(toSave));
        last.Start();
        setSaveThreads.Add(last);
        return setSaveThreads;
    }
    private void ThreadSaveSettlements(Settlement[] sets)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string setDir = PersistentDataPath + "/" + SaveFile + "/WorldSettlements/";

        foreach (Settlement s in sets)
        {
            if (s == null) //If null, we've finished
                return;
            FileStream set = File.Create(setDir + s.SettlementID + ".save");
            bf.Serialize(new BufferedStream(set), s);
            //bf.Serialize(set, s);
            set.Close();
        }
    }
    private void ThreadSaveKingdoms(GameLoadSave gls)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string kingdomFile = PersistentDataPath + "/" + SaveFile + "/WorldKingdoms.save";
        FileStream king = File.Create(kingdomFile);
        bf.Serialize(king, gls.WorldKingdoms);
        king.Close();
    }
    private void ThreadSaveEntity(GameLoadSave gls)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string entityFile = PersistentDataPath + "/" + SaveFile + "/Entities1.save";
        string entityFile2 = PersistentDataPath + "/" + SaveFile + "/Entities2.save";
        FileStream ent1 = File.Create(entityFile);
        bf.Serialize(ent1, gls.GameEntities);
        ent1.Close();

        FileStream ent2 = File.Create(entityFile2);
        bf.Serialize(ent2, gls.GameEntityChunks);
        ent2.Close();
    }
   

    public GameLoadSave Load()
    {
        string filePath = PersistentDataPath + "/" + SaveFile + "/World.save";
        string kingdomFile = PersistentDataPath  + "/" + SaveFile + "/WorldKingdoms.save";
        string settlementFile = PersistentDataPath + "/" + SaveFile + "/WorldSettlements.save";
        string entityFile = PersistentDataPath + "/" + SaveFile + "/Entities1.save";
        string entityFile2 = PersistentDataPath + "/" + SaveFile + "/Entities2.save";

        GameLoadSave gls = new GameLoadSave(null);

        BinaryFormatter bf = new BinaryFormatter();

        //Thread kThread = new Thread(() => ThreadLoadKingdoms(gls, kingdomFile));
        //kThread.Start();

        UnityEngine.Profiling.Profiler.BeginSample("kingdom_deserialisation");
        if (File.Exists(kingdomFile))
        {
            FileStream file = File.Open(kingdomFile, FileMode.Open);
            gls.WorldKingdoms = (Dictionary<int, Kingdom>)bf.Deserialize(file);
        }
        UnityEngine.Profiling.Profiler.EndSample();
        UnityEngine.Profiling.Profiler.BeginSample("set_deserialisation");

        if (File.Exists(settlementFile))
        {
            FileStream file = File.Open(settlementFile, FileMode.Open);
            gls.WorldSettlements = (Dictionary<int, Settlement>)bf.Deserialize(file);
        }
        UnityEngine.Profiling.Profiler.EndSample();
        UnityEngine.Profiling.Profiler.BeginSample("entity_deseri");
        if (File.Exists(entityFile))
        {
            FileStream file = File.Open(entityFile, FileMode.Open);
            gls.GameEntities = (Dictionary<int, Entity>)bf.Deserialize(file);
        }
        if (File.Exists(entityFile2))
        {
            FileStream file = File.Open(entityFile2, FileMode.Open);
            gls.GameEntityChunks = (Dictionary<Vec2i, List<int>>)bf.Deserialize(file);
        }
        UnityEngine.Profiling.Profiler.EndSample();

        //kThread.
        return gls;
    }

    private void ThreadLoadKingdoms(GameLoadSave gls, string path)
    {
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            gls.WorldKingdoms = (Dictionary<int, Kingdom>)bf.Deserialize(file);
        }
    }
    
  
}


[System.Serializable]
public class GameLoadSave
{
    [System.NonSerialized]
    private WorldManager WorldManager;
    [SerializeField]
    public ChunkRegion[,] ChunkRegions;
    [SerializeField]
    public Dictionary<int, Settlement> WorldSettlements;
    [SerializeField]
    public Dictionary<int, Kingdom> WorldKingdoms;
    [SerializeField]
    public Dictionary<int, Entity> GameEntities;
    [SerializeField]
    public Dictionary<Vec2i, List<int>> GameEntityChunks;

    public GameLoadSave(WorldManager worldManager)
    {
        WorldManager = worldManager;
    }

    public void GatherData()
    {
        //Get settlement and kingdom data from world
        WorldManager.World.WorldSave(this);
        GameManager.EntityManager.Save(this);
        ChunkRegions = WorldManager.LoadedRegions;

    }
    public World GetWorld()
    {
        return WorldManager.World;
    }
}