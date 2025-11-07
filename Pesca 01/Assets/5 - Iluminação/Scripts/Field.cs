using System.Collections.Generic;
using UnityEngine;
using TMPro; // Recomenda-se TextMeshPro para UI

public class Field : MonoBehaviour
{
  // Referência para o texto da UI (Arraste no Inspector)
  [Header("UI (Opcional)")]
  public TextMeshProUGUI statusText; // Use TextMeshProUGUI (Recomendado)
  // public UnityEngine.UI.Text statusText; // Use isto se for o UI.Text legado
  [Header("UI Panels")]
  public GameObject panel;
    
  [Header("Field")]
    public GameObject field;
  private Tile[,] _grid;
  private bool _canDrawConnection = false;

  private List<Tile> _connections = new List<Tile>();
  private Tile _connectionTile;

  private List<int> _solvedConnections = new List<int>();

  private int _dimensionX = 0; // Colunas (Tiles)
  private int _dimensionY = 0; // Linhas (Rows)
  private int _solved = 0;
  private Dictionary<int, int> _amountToSolve = new Dictionary<int, int>();

  void Start()
  {
    Cursor.visible = true;
    Cursor.lockState = CursorLockMode.None;
    // --- GARANTA QUE A CORREÇÃO X/Y ESTÁ APLICADA ---
    // (O seu OnMouseDown() funcionar sugere que esta parte já está correta no seu ficheiro)
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
    // --- FIM DA VERIFICAÇÃO ---

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
    // Só desenha conexões se o mouse estiver pressionado e o jogador puder desenhar
    if (!_canDrawConnection) return;

    // Se o botão do mouse foi solto, parar o arrasto
    if (!Input.GetMouseButton(0))
    {
        _canDrawConnection = false;
        return;
    }

    if (Camera.main == null) return;

    // --- Conversão correta do mouse para posição local do grid ---
    Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    Vector3 localPos = transform.InverseTransformPoint(mouseWorld);

    // Cada tile ocupa 1 unidade (ajuste o +0.5f se precisar alinhar visualmente)
    _mouseGridX = Mathf.FloorToInt(localPos.x + 0.5f);
    _mouseGridY = Mathf.FloorToInt(localPos.y + 0.5f);

    // Debug opcional: veja se os índices estão corretos
    // Debug.Log($"MouseLocal: {localPos} -> GridIndex: ({_mouseGridX}, {_mouseGridY})");

    // Impedir acesso fora dos limites da matriz
    if (_mouseGridX < 0 || _mouseGridX >= _dimensionX ||
        _mouseGridY < 0 || _mouseGridY >= _dimensionY)
    {
        return;
    }

    Tile hoverTile = _grid[_mouseGridX, _mouseGridY];
    if (hoverTile == null) return;

    if (_connections == null || _connections.Count == 0)
    {
        _canDrawConnection = false;
        return;
    }

    Tile firstTile = _connections[0];
    bool isDifferentActiveTile = hoverTile.cid > 0 && hoverTile.cid != firstTile.cid;

    // Impede sobrescrever tiles já resolvidos ou errados
    if (hoverTile.isHighlighted || hoverTile.isSolved || isDifferentActiveTile) return;

    Vector2 connectionTilePosition = _FindTileCoordinates(_connectionTile);
    bool isPositionDifferent = IsDifferentPosition(_mouseGridX, _mouseGridY, connectionTilePosition);

    if (isPositionDifferent)
    {
        var deltaX = Mathf.Abs(connectionTilePosition.x - _mouseGridX);
        var deltaY = Mathf.Abs(connectionTilePosition.y - _mouseGridY);

        bool isShiftNotOnNext = deltaX > 1 || deltaY > 1;
        bool isShiftDiagonal = (deltaX > 0 && deltaY > 0);

        if (isShiftNotOnNext || isShiftDiagonal) return;

        // --- CHAMA O HIGHLIGHT VISUALMENTE ---
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

        // Verifica se os tiles se conectam corretamente
        if (_CheckIfTilesMatch(hoverTile, firstTile))
        {
            _connections.ForEach(tile => tile.isSolved = true);
            _canDrawConnection = false;
            _amountToSolve.Remove(firstTile.cid);
            SetGameStatus(++_solved, _amountToSolve.Count + _solved);

            if (_amountToSolve.Keys.Count == 0)
            {
                Debug.Log("🎉 Ganhou!");
                panel.SetActive(true);
                field.SetActive(false);
                if (GameProgressManager.Instance != null)
                {
                    GameProgressManager.Instance.RegisterGamePhaseCompleted();
                }
                else
                {
                    Debug.LogWarning("GameProgressManager.Instance não encontrado. Não foi possível registar a conclusão da fase.");
                }
            }
        }
    }
}



  bool _CheckIfTilesMatch(Tile tile, Tile another)
  {
    return tile.cid > 0 && another.cid == tile.cid;
  }

  bool _CheckMouseOutsideGrid()
  {
    // Verifica se o índice do rato está dentro dos limites do array
    return _mouseGridY >= _dimensionY || _mouseGridY < 0 || _mouseGridX >= _dimensionX || _mouseGridX < 0;
  }

  void onTileSelected(Tile tile)
  {
    // --- DEBUG ADICIONADO ---
    Debug.Log($"FIELD: onTileSelected() chamado. isSelected = {tile.isSelected}");

    if (tile.isSelected)
    {
      _connectionTile = tile;
      _connections = new List<Tile>();
      _connections.Add(_connectionTile);
      _canDrawConnection = true;
      _connectionTile.Highlight(); // <-- Esta chamada deve funcionar agora
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
    else
    {
        GameObject statusTextObj = GameObject.Find("txtStatus");
        if (statusTextObj != null)
        {
            var tmp = statusTextObj.GetComponent<TextMeshProUGUI>();
            if (tmp != null)
            {
                statusText = tmp; 
                statusText.text = "Resolvido: " + solved + " de " + from;
            }
            else
            {
                var legacyText = statusTextObj.GetComponent<UnityEngine.UI.Text>();
                if (legacyText != null)
                {
                    legacyText.text = "Resolvido: " + solved + " de " + from;
                }
            }
        }
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