using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Aujourd'hui
// 1) Pratique: objet qui visite une liste de destinations
// 2) Vague d'ennemis qui suit le chemin le plus court
// 3) Détecter un chemin bloqué

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameTilePrefab;
    [SerializeField] GameObject enemyPrefab;
    GameTile[,] gameTiles;
    private GameTile spawnTile;
    const int ColCount = 20;
    const int RowCount = 10;

    public GameTile TargetTile { get; internal set; }
    List<GameTile> pathToGoal = new List<GameTile>();

    private void Awake()
    {
        gameTiles = new GameTile[ColCount, RowCount];

        for (int x = 0; x < ColCount; x++)
        {
            for (int y = 0; y < RowCount; y++)
            {
                var spawnPosition = new Vector3(x, y, 0);
                var tile = Instantiate(gameTilePrefab, spawnPosition, Quaternion.identity);
                gameTiles[x, y] = tile.GetComponent<GameTile>();
                gameTiles[x, y].GM = this;
                gameTiles[x, y].X = x;
                gameTiles[x, y].Y = y;

                if ((x + y) % 2 == 0)
                {
                    gameTiles[x, y].TurnGrey();
                }
            }
        }

        spawnTile = gameTiles[1, 7];
        spawnTile.SetEnemySpawn();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && TargetTile != null) 
        {
            foreach (var t in gameTiles)
            {
                t.SetPath(false);
            }

            var path = Pathfinding(spawnTile, TargetTile);
            var tile = TargetTile;

            while (tile != null)
            {
                pathToGoal.Add(tile);
                tile.SetPath(true);
                tile = path[tile];
            }
            StartCoroutine(SpawnEnemyCoroutine());
        }
    }

    private Dictionary<GameTile, GameTile> Pathfinding(GameTile sourceTile, GameTile targetTile)
    {
        // distance minimal de la tuile a la source
        var dist = new Dictionary<GameTile, int>();

        // tuile precedente qui mene au chemin le plus court
        var prev = new Dictionary<GameTile, GameTile>();

        // liste des tuiles restante
        var Q = new List<GameTile>();

//3      for each vertex v in Graph.Vertices:
        foreach (var v in gameTiles)
        {
//4          dist[v] ← INFINITY
            dist.Add(v, 9999);

//5          prev[v] ← UNDEFINED
            prev.Add(v, null);

//6          add v to Q
            Q.Add(v);
        }

//7      dist[source] ← 0
        dist[sourceTile] = 0;

//8
//9      while Q is not empty:
        while (Q.Count > 0)
        {
//10          u ← vertex in Q with min dist[u]
            GameTile u = null;
            int minDistance = int.MaxValue;

            foreach (var v in Q)
            {
                if (dist[v] < minDistance)
                {
                    minDistance = dist[v];
                    u = v;
                }
            }

//11          remove u from Q
            Q.Remove(u);
            //12
            //13          for each neighbor v of u still in Q:
            foreach (var v in FindNeighbor(u))
            {
                if (!Q.Contains(v) || v.IsBlocked)
                {
                    continue;
                }

//14              alt ← dist[u] + Graph.Edges(u, v)
                int alt = dist[u] + 1;

//15              if alt < dist[v]:
                if (alt < dist[v])
                {
//16                  dist[v] ← alt
                    dist[v] = alt;
//17                  prev[v] ← u
                    prev[v] = u;
                }
            }
        }

//19      return dist[], prev[]
        return prev;
    }

    private List<GameTile> FindNeighbor(GameTile u)
    {
        var result = new List<GameTile>();

        if (u.X - 1 >= 0)
            result.Add(gameTiles[u.X - 1, u.Y]);
        if (u.X + 1 < ColCount)
            result.Add(gameTiles[u.X + 1, u.Y]);
        if (u.Y - 1 >= 0)
            result.Add(gameTiles[u.X, u.Y - 1]);
        if (u.Y + 1 < RowCount)
            result.Add(gameTiles[u.X, u.Y + 1]);

        return result;
    }

    IEnumerator SpawnEnemyCoroutine()
    {
        while (true)
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(0.6f);
                var enemy = Instantiate(enemyPrefab, spawnTile.transform.position, Quaternion.identity);
                enemy.GetComponent<Enemy>().SetPath(pathToGoal);
            }
            yield return new WaitForSeconds(2f);
        }
    }
}
