using System.Collections.Generic;
using UnityEngine;
using TMPro; // Recomenda-se TextMeshPro para UI
// using UnityEngine.SceneManagement; // Removido, o GameProgressManager trata disto

public class Field : MonoBehaviour
{
  // Referência para o texto da UI (Arraste no Inspector)
  [Header("UI (Opcional)")]
  public TextMeshProUGUI statusText; 
  // public UnityEngine.UI.Text statusText; // Use isto se for o UI.Text legado

  private Tile[,] _grid;
  private bool _canDrawConnection = false;
  private bool _isComplete = false; // Para garantir que "Ganhou" só é chamado uma vez

  private List<Tile> _connections = new List<Tile>();
  private Tile _connectionTile;

  private List<int> _solvedConnections = new List<int>();

  private int _dimensionX = 0; // Colunas (Tiles)
  private int _dimensionY = 0; // Linhas (Rows)
  private int _solved = 0;
  private Dictionary<int, int> _amountToSolve = new Dictionary<int, int>();

  void Start()
  {
    // Força o cursor a aparecer nesta cena
    Cursor.visible = true;
    Cursor.lockState = CursorLockMode.None;

    // --- CORREÇÃO DE LÓGICA (X e Y estavam invertidos) ---
    _dimensionY = transform.childCount; 
    if (_dimensionY == 0)
    {
        Debug.LogError("O objeto Field não tem filhos (Linhas). Configure a hierarquia.");
        return;
    }
    _dimensionX = transform.GetChild(0).transform.childCount; 
    _grid = new Tile[_dimensionX, _dimensionY]; 

    for (int y = 0; y < _dimensionY; y++)
    {
        var row = transform.GetChild(y).transform;
        row.gameObject.name = "" + y;
        
        for (int x = 0; x < _dimensionX; x++)
        {
            var tile = row.GetChild(x).GetComponent<Tile>();
            tile.gameObject.name = "" + x;
            tile.onSelected.AddListener(onTileSelected);
            _CollectAmountToSolveFromTile(tile);
            _grid[x, y] = tile;
        }
    }
    // --- FIM DA CORREÇÃO ---

    SetGameStatus(_solved, _amountToSolve.Count);
    _OutputGrid();
  }

  void _CollectAmountToSolveFromTile(Tile tile)
  {
    if (tile.cid > Tile.UNPLAYABLE_INDEX)
    {
      if (_amountToSolve.ContainsKey(tile.cid))
        _amountToSolve[tile.cid] += 1;
      else _amountToSolve[tile.cid] = 1;
    }
  }

  void _OutputGrid()
  {
    var results = "";
    for (int y = 0; y < _dimensionY; y++)
    {
      results += "{";
      for (int x = 0; x < _dimensionX; x++)
      {
        var tile = _grid[x, y];
        if (x > 0) results += ",";
        results += tile.cid;
      }
      results += "}\n";
    }
    Debug.Log("Main -> Start: _grid: \n" + results);
  }

  Vector3 _mouseWorldPosition;
  int _mouseGridX, _mouseGridY;

  void Update()
  {
    if (_isComplete || !_canDrawConnection) return;
    
    if (Camera.main == null) return;
            
    _mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    
    // CORRIGIDO: Usar Round (Arredondar) é mais preciso
    _mouseGridX = (int)Mathf.Round(_mouseWorldPosition.x);
    _mouseGridY = (int)Mathf.Round(_mouseWorldPosition.y);
    
    if (_CheckMouseOutsideGrid()) return;
    
    if (_grid[_mouseGridX, _mouseGridY] == null) return;

    Tile hoverTile = _grid[_mouseGridX, _mouseGridY];
    
    if (_connections == null || _connections.Count == 0)
    {
         _canDrawConnection = false; 
         return;
    }
    
    Tile firstTile = _connections[0];
    bool isDifferentActiveTile = hoverTile.cid > 0 && hoverTile.cid != firstTile.cid;

    if (hoverTile.isHighlighted || hoverTile.isSolved || isDifferentActiveTile) return;

    Vector2 connectionTilePosition = _FindTileCoordinates(_connectionTile);
    bool isPositionDifferent = IsDifferentPosition(_mouseGridX, _mouseGridY, connectionTilePosition);

    if (isPositionDifferent)
    {
      var deltaX = System.Math.Abs(connectionTilePosition.x - _mouseGridX);
      var deltaY = System.Math.Abs(connectionTilePosition.y - _mouseGridY);
      bool isShiftNotOnNext = deltaX > 1 || deltaY > 1;
      bool isShiftDiagonal = (deltaX > 0 && deltaY > 0);
      
      if (isShiftNotOnNext || isShiftDiagonal) return;

      hoverTile.Highlight();
      hoverTile.SetConnectionColor(_connectionTile.ConnectionColor);

      _connectionTile.ConnectionToSide(
        _mouseGridY > connectionTilePosition.y,
        _mouseGridX > connectionTilePosition.x,
        _mouseGridY < connectionTilePosition.y,
        _mouseGridX < connectionTilePosition.x
      );

      _connectionTile = hoverTile;
      _connections.Add(_connectionTile);

      if (_CheckIfTilesMatch(hoverTile, firstTile))
      {
        _connections.ForEach((tile) => tile.isSolved = true);
        _canDrawConnection = false;
        _amountToSolve.Remove(firstTile.cid);
        SetGameStatus(++_solved, _amountToSolve.Count + _solved);
        
        if (_amountToSolve.Keys.Count == 0)
        {
          CompleteGame();
        }
      }
    }
  }

  // ATUALIZADO: Chama o GameProgressManager para voltar
  void CompleteGame()
  {
      if (_isComplete) return; 
      _isComplete = true;

      Debug.Log("Ganhou! A notificar o GameProgressManager...");

      if (GameProgressManager.Instance != null)
      {
          // 1. Regista que a fase foi concluída
          GameProgressManager.Instance.RegisterGamePhaseCompleted();
          
          // 2. Manda o gestor descarregar esta cena (o minigame) e voltar à principal
          GameProgressManager.Instance.ReturnToMainScene(); 
      }
      else
      {
          Debug.LogError("GameProgressManager.Instance não encontrado!");
      }
  }

  bool _CheckIfTilesMatch(Tile tile, Tile another)
  {
    return tile.cid > 0 && another.cid == tile.cid;
  }

  bool _CheckMouseOutsideGrid()
  {
    return _mouseGridY >= _dimensionY || _mouseGridY < 0 || _mouseGridX >= _dimensionX || _mouseGridX < 0;
  }

  void onTileSelected(Tile tile)
  {
    if (tile.isSelected)
    {
      _connectionTile = tile;
      _connections = new List<Tile>();
      _connections.Add(_connectionTile);
      _canDrawConnection = true;
      _connectionTile.Highlight();
    }
    else
    {
      if (_connectionTile == null) 
      {
          _canDrawConnection = false;
          return;
      }
      bool isFirstTileInConnection = (_connectionTile == tile);
      if (isFirstTileInConnection) 
      {
          tile.HightlightReset();
      }
      else if (!_CheckIfTilesMatch(_connectionTile, tile))
      {
          _ResetConnections();
      }
      _canDrawConnection = false;
    }
  }

  public void onRestart()
  {
    Debug.Log("Field -> onRestart");
    _isComplete = false; 
    
    for (int y = 0; y < _dimensionY; y++)
    {
      for (int x = 0; x < _dimensionX; x++)
      {
        var tile = _grid[x, y];
        tile.ResetConnection();
        tile.HightlightReset();
        _CollectAmountToSolveFromTile(tile);
      }
    }
    _solved = 0;
    SetGameStatus(_solved, _amountToSolve.Count);
  }

  void SetGameStatus(int solved, int from)
  {
    if (statusText != null)
    {
        statusText.text = "Resolvido: " + solved + " de " + from;
    }
  }

  void _ResetConnections()
  {
    if (_connections == null) return;

    _connections.ForEach((tile) =>
    {
      tile.ResetConnection();
      tile.HightlightReset();
    });
  }

  Vector2 _FindTileCoordinates(Tile tile)
  {
    int x = int.Parse(tile.gameObject.name);
    int y = int.Parse(tile.gameObject.transform.parent.gameObject.name);
    return new Vector2(x, y);
  }

  public bool IsDifferentPosition(int gridX, int gridY, Vector2 position)
  {
    return position.x != gridX || position.y != gridY;
  }

  // Classe interna não utilizada, mas inofensiva
  private class Connection
  {
    public Tile tile;
    public Vector2 position;
    public Connection(Tile tile, Vector2 position)
    {
      this.tile = tile;
      this.position = position;
    }
    public bool IsDifferentPosition(int gridX, int gridY)
    {
      return this.position.x != gridX || this.position.y != gridY;
    }
  }
}