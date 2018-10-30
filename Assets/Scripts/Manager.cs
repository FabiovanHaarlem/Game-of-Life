using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [SerializeField]
    private int m_GridWidth;
    [SerializeField]
    private int m_GridHeight;
    [SerializeField]
    private int m_TowerHeight;

    private Block[,] m_Blocks;
    private GameObject[] m_TowerBlocksReserve;
    private List<GameObject> m_TowerBlocksUsed;

    [SerializeField]
    private GameObject m_StartButton;
    [SerializeField]
    private GameObject m_Restart;
    [SerializeField]
    private GameObject m_MakeGridButton;
    [SerializeField]
    private Camera m_Camera;
    [SerializeField]
    private Slider m_WidthSlider;
    [SerializeField]
    private Slider m_HeightSlider;
    [SerializeField]
    private Text m_WidthText;
    [SerializeField]
    private Text m_HeightText;
    [SerializeField]
    private GameObject m_BuildToggle;
    [SerializeField]
    private Slider m_TowerHeightSlider;
    [SerializeField]
    private Text m_TowerHeightText;
    [SerializeField]
    private CameraMovement m_CameraMovement;
    [SerializeField]
    private GameObject m_MovementText;

    private bool m_GameStarted;
    [SerializeField]
    private bool m_BuildTower;
    private int m_IndexOfTower;
	
	void Start ()
    {
        m_Restart.SetActive(false);
        m_StartButton.SetActive(false);
        m_MovementText.SetActive(false);
        m_GameStarted = false;
        m_BuildTower = false;
        m_TowerHeightSlider.gameObject.SetActive(false);
        m_WidthSlider.value = 5;
        m_HeightSlider.value = 5;
        m_TowerHeight = 1;
        m_TowerHeight = (int)m_TowerHeightSlider.value;
        m_GridWidth = (int)m_WidthSlider.value;
        m_GridHeight = (int)m_HeightSlider.value;
        m_WidthText.text = "Grid Width = " + m_GridWidth;
        m_HeightText.text = "Grid Height = " + m_GridHeight;
        m_TowerHeightText.text = "Tower Height = " + m_TowerHeight;

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }


        if (Input.GetMouseButton(0))
        {
            SelectBlock();
        }

        else if (m_GameStarted)
        {
            CheckBlocks();

            for (int y = 0; y < m_GridHeight; y++)
            {
                for (int x = 0; x < m_GridWidth; x++)
                {
                    if (m_Blocks[y, x].CheckIfOnEndge() != null)
                    {
                        m_Blocks[y, x].UpdateBlock();
                    }
                }
            }
            if (m_BuildTower)
            {
                if (m_IndexOfTower <= m_TowerHeight)
                {
                    for (int i = 0; i < m_TowerBlocksUsed.Count; i++)
                    {
                        m_TowerBlocksUsed[i].transform.position = new Vector3(m_TowerBlocksUsed[i].transform.position.x, m_TowerBlocksUsed[i].transform.position.y + 1f, m_TowerBlocksUsed[i].transform.position.z);
                    }
                    m_IndexOfTower += 1;
                }
            }
        }
    }

    private void CheckBlocks()
    {
        for (int y = 0; y < m_GridHeight; y++)
        {
            for (int x = 0; x < m_GridWidth; x++)
            {
                if (m_Blocks[y, x].CheckIfOnEndge() != null)
                {
                    int aliveBlocks = 0;

                    if (m_Blocks[y - 1, x - 1].GetStatus() == true)
                    {
                        aliveBlocks += 1;
                    }
                    if (m_Blocks[y - 1, x].GetStatus() == true)
                    {
                        aliveBlocks += 1;
                    }
                    if (m_Blocks[y - 1, x + 1].GetStatus() == true)
                    {
                        aliveBlocks += 1;
                    }
                    if (m_Blocks[y, x - 1].GetStatus() == true)
                    {
                        aliveBlocks += 1;
                    }
                    if (m_Blocks[y, x + 1].GetStatus() == true)
                    {
                        aliveBlocks += 1;
                    }
                    if (m_Blocks[y + 1, x - 1].GetStatus() == true)
                    {
                        aliveBlocks += 1;
                    }
                    if (m_Blocks[y + 1, x].GetStatus() == true)
                    {
                        aliveBlocks += 1;
                    }
                    if (m_Blocks[y + 1, x + 1].GetStatus() == true)
                    {
                        aliveBlocks += 1;
                    }

                    if (aliveBlocks >= 4 || aliveBlocks <= 1)
                    {
                        m_Blocks[y, x].WillBeAlive(false);
                    }
                    else if (aliveBlocks == 3)
                    {
                        m_Blocks[y, x].WillBeAlive(true);

                        if (m_BuildTower)
                        {
                            for (int i = 0; i < m_TowerBlocksReserve.Length; i++)
                            {
                                if (!m_TowerBlocksReserve[i].activeInHierarchy)
                                {
                                    m_TowerBlocksReserve[i].SetActive(true);
                                    m_TowerBlocksReserve[i].transform.position = new Vector3(m_Blocks[y, x].transform.position.x, m_Blocks[y, x].transform.position.y, m_Blocks[y, x].transform.position.z);
                                    m_TowerBlocksUsed.Add(m_TowerBlocksReserve[i]);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void SelectBlock()
    {
        RaycastHit hit;

        Ray ray = (Camera.main.ScreenPointToRay(Input.mousePosition));

        Debug.DrawRay(Camera.main.transform.position, ray.direction * 50, Color.red);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.CompareTag("Block"))
            {
                for (int y = 0; y < m_GridHeight; y++)
                {
                    for (int x = 0; x < m_GridWidth; x++)
                    {
                        if (hit.collider.gameObject.GetInstanceID() == m_Blocks[y, x].gameObject.GetInstanceID())
                        {
                            if (m_Blocks[y, x].CheckIfOnEndge() != null)
                            {
                                m_Blocks[y, x].ComeAlive();
                            }
                            break;
                        }
                    }
                }
            }
        }
    }

    public void StartGame()
    {
        m_GameStarted = true;
        m_StartButton.SetActive(false);
        m_Restart.SetActive(true);
        m_CameraMovement.SetRightCamera(m_BuildTower);

        if (m_BuildTower)
        {
            m_MovementText.SetActive(true);
        }
        else if (!m_BuildTower)
        {
            m_MovementText.SetActive(false);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EnableTowerBuilding(bool enableTowerBuilding)
    {
        if (!m_GameStarted)
        {
            m_BuildTower = enableTowerBuilding;
        }

        if (enableTowerBuilding)
        {
            m_TowerHeightSlider.gameObject.SetActive(true);
        }
        else
        {
            m_TowerHeightSlider.gameObject.SetActive(false);
        }
    }

    public void MakeGrid()
    {
        if (m_GridWidth != m_GridHeight)
        {
            m_GridWidth = m_GridHeight;
        }

        m_IndexOfTower = 0;
        m_Camera.fieldOfView = (m_GridHeight + m_GridWidth);

        m_Blocks = new Block[m_GridWidth, m_GridHeight];
        m_TowerBlocksReserve = new GameObject[((m_GridWidth * m_GridHeight) / 2) * m_TowerHeight];
        m_TowerBlocksUsed = new List<GameObject>();

        for (int y = 0; y < m_GridHeight; y++)
        {
            for (int x = 0; x < m_GridWidth; x++)
            {
                GameObject block = Instantiate(Resources.Load<GameObject>("Prefabs/Block"));
                block.transform.position = new Vector3((-m_GridWidth / 2f) + (1f * (x + 1f)), 0.5f, -(m_GridHeight / 2f) + (1f * (y + 1f)));
                m_Blocks[x, y] = block.GetComponent<Block>();

                if (y == 0)
                {
                    m_Blocks[x, y].Initialize(true);
                    m_Blocks[x, y].IsOnEdge();
                }
                else if (x == 0)
                {
                    m_Blocks[x, y].Initialize(true);
                    m_Blocks[x, y].IsOnEdge();
                }
                else if (y == m_GridHeight - 1)
                {
                    m_Blocks[x, y].Initialize(true);
                    m_Blocks[x, y].IsOnEdge();
                }
                else if (x == m_GridWidth - 1)
                {
                    m_Blocks[x, y].Initialize(true);
                    m_Blocks[x, y].IsOnEdge();
                }
                else
                {
                    m_Blocks[x, y].Initialize(false);
                    m_Blocks[x, y].Die();
                }
            }
        }

        for (int i = 0; i < ((m_GridWidth * m_GridHeight) / 2) * m_TowerHeight; i++)
        {
            m_TowerBlocksReserve[i] = Instantiate(Resources.Load<GameObject>("Prefabs/TowerBlock"));
            m_TowerBlocksReserve[i].SetActive(false);
        }
        m_StartButton.SetActive(true);
        m_MakeGridButton.SetActive(false);
        m_WidthSlider.gameObject.SetActive(false);
        m_HeightSlider.gameObject.SetActive(false);
        m_BuildToggle.SetActive(false);
        m_TowerHeightSlider.gameObject.SetActive(false);
    }

    public void GetGridWidth(int gridWidth)
    {
        m_GridWidth = (int)m_WidthSlider.value;
        m_WidthText.text = "Grid Width = " + m_GridWidth;
    }

    public void GetGridHeight(int gridHeight)
    {
        m_GridHeight = (int)m_HeightSlider.value;
        m_HeightText.text = "Grid Height = " + m_GridHeight;
    }

    public void GetTowerHeight(int towerHeight)
    {
        m_TowerHeight = (int)m_TowerHeightSlider.value;
        m_TowerHeightText.text = "Tower Height = " + m_TowerHeight;

    }
}
