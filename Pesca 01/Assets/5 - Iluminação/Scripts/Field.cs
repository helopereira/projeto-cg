using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class Field : MonoBehaviour
{
    [Header("UI (Opcional)")]
    public TextMeshProUGUI statusText; 

    private Tile[,] _grid;
    private bool _isComplete = false; 

    private List<Tile> _connections = new List<Tile>();
    private Tile _connectionTile;

    private int _dimensionX = 0; 
    private int _dimensionY = 0; 
    private int _solved = 0;
    private Dictionary<int, int> _amountToSolve = new Dictionary<int, int>();

    private int _lastGridX = -1;
    private int _lastGridY = -1;

    void Awake() // Inicialização movida para Awake()
    {
        Time.timeScale = 1.0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Limpeza: Desativa o AudioListener duplicado (se houver)
        var listeners = FindObjectsOfType<AudioListener>();
        if (listeners.Length > 1)
        {
            AudioListener myListener = GetComponent<AudioListener>();
            if (myListener != null) myListener.enabled = false;
        }

        // --- Lógica de Inicialização da Grade ---
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
        
        SetGameStatus(_solved, _amountToSolve.Count); 
        _OutputGrid();
    }

    void Start() 
    {
        // Opcional
    }

    private Vector3 _mouseWorldPosition;
    private int _mouseGridX, _mouseGridY;

    void Update()
    {
        // Garante que o arrasto só ocorre se há uma conexão ativa e o botão está pressionado
        if (_isComplete || _connections.Count == 0 || !Input.GetMouseButton(0)) return; 
        
        if (Camera.main == null) return;
        
        _mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        _mouseGridX = (int)Mathf.Round(_mouseWorldPosition.x);
        _mouseGridY = (int)Mathf.Round(_mouseWorldPosition.y);
        
        if (_CheckMouseOutsideGrid()) return;
        if (_grid[_mouseGridX, _mouseGridY] == null) return;

        Tile hoverTile = _grid[_mouseGridX, _mouseGridY];
        Tile firstTile = _connections[0];

        // Se o mouse está no mesmo tile que o anterior, não faz nada
        if (_mouseGridX == _lastGridX && _mouseGridY == _lastGridY) return;

        bool isDifferentActiveTile = hoverTile.cid > 0 && hoverTile.cid != firstTile.cid;

        // Se o tile já está destacado/resolvido OU é de outra cor, para
        if (hoverTile.isHighlighted || hoverTile.isSolved || isDifferentActiveTile) return;

        Vector2 connectionTilePosition = _FindTileCoordinates(_connectionTile);
        
        // Lógica de Adjacência (impede saltos e diagonais)
        var deltaX = System.Math.Abs(connectionTilePosition.x - _mouseGridX);
        var deltaY = System.Math.Abs(connectionTilePosition.y - _mouseGridY);
        bool isShiftNotOnNext = deltaX > 1 || deltaY > 1;
        bool isShiftDiagonal = (deltaX > 0 && deltaY > 0);
        
        if (isShiftNotOnNext || isShiftDiagonal) return;

        // Movimento válido: Desenha a conexão
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
        
        _lastGridX = _mouseGridX;
        _lastGridY = _mouseGridY;

        // Verifica se a conexão terminou
        if (_CheckIfTilesMatch(hoverTile, firstTile))
        {
            _connections.ForEach((tile) => tile.isSolved = true);
            _amountToSolve.Remove(firstTile.cid);
            SetGameStatus(++_solved, _amountToSolve.Count + _solved);
            
            // CORREÇÃO: Limpa a lista de conexões para interromper o arrasto imediatamente
            _connections.Clear(); 

            if (_amountToSolve.Keys.Count == 0)
            {
                CompleteGame();
            }
            
            // Interrompe o Update neste frame para garantir que nada mais seja desenhado
            return; 
        }
    }

    // --- MÉTODOS AUXILIARES ---

    private void _CollectAmountToSolveFromTile(Tile tile)
    {
        if (tile.cid > Tile.UNPLAYABLE_INDEX)
        {
            if (_amountToSolve.ContainsKey(tile.cid))
                _amountToSolve[tile.cid] += 1;
            else _amountToSolve[tile.cid] = 1;
        }
    }

    private void _OutputGrid()
    {
        // Implementação da saída da grade (para Debug)
    }

    private bool _CheckIfTilesMatch(Tile tile, Tile another)
    {
        return tile.cid > 0 && another.cid == tile.cid;
    }

    private bool _CheckMouseOutsideGrid()
    {
        return _mouseGridY >= _dimensionY || _mouseGridY < 0 || _mouseGridX >= _dimensionX || _mouseGridX < 0;
    }

    private Vector2 _FindTileCoordinates(Tile tile)
    {
        int x = int.Parse(tile.gameObject.name);
        int y = int.Parse(tile.gameObject.transform.parent.gameObject.name);
        return new Vector2(x, y);
    }
    
    private void _ResetConnections()
    {
        if (_connections == null) return;

        _connections.ForEach((tile) =>
        {
            tile.ResetConnection();
            tile.HightlightReset();
        });
        
        // NOVO: Limpa a lista de conexões ao resetar
        _connections.Clear(); 
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
    
    public bool IsDifferentPosition(int gridX, int gridY, Vector2 position)
    {
        return position.x != gridX || position.y != gridY;
    }

    void CompleteGame()
    {
        if (_isComplete) return; 
        _isComplete = true;

        if (GameProgressManager.Instance != null)
        {
            GameProgressManager.Instance.RegisterGamePhaseCompleted();
            GameProgressManager.Instance.ReturnToMainScene(); 
        }
        else
        {
            Debug.LogError("GameProgressManager.Instance não encontrado!");
        }
    }

    void onTileSelected(Tile tile)
    {
        if (tile.isSelected) // OnMouseDown
        {
            // Se já houver conexões pendentes, reseta
            if (_connections.Count > 0)
            {
                _ResetConnections();
            }
            
            _connectionTile = tile;
            _connections = new List<Tile>();
            _connections.Add(_connectionTile);
            _connectionTile.Highlight();
            
            Vector2 pos = _FindTileCoordinates(tile);
            _lastGridX = (int)pos.x;
            _lastGridY = (int)pos.y;
        }
        else // OnMouseUp
        {
            // Garante que, se o mouse for solto e o par não estiver completo, o desenho é apagado
            if (_connections.Count > 0 && !_CheckIfTilesMatch(_connections[0], _connectionTile))
            {
                _ResetConnections();
            }
            else
            {
                // Se soltou e o par foi completado com sucesso, apenas limpa as posições
                _connections.Clear();
            }
            _lastGridX = -1;
            _lastGridY = -1;
        }
    }

    private void SetGameStatus(int solved, int from)
    {
        if (statusText != null)
        {
            statusText.text = "Resolvido: " + solved + " de " + from;
        }
    }
}