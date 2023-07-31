using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Qad : MonoBehaviour
{


    [Header("Render controling variables and game loop logic")]
    public bool walkable = true;
    public bool current = false;
    public bool target = false;
    public bool selectable = false;
    public bool attacked = false;
    public bool diagonalToCurrent = false;
    public bool playerAttack = false;

    public Material qadMat;

    public Renderer qadRender;
    public Transform qadRenderTransform;

    public GameObject currentPiece;
    public int currentPieceType;

    public float qadDamage = 0;

    [Header("Pathfinding help variables")]
    //Stores every adjacent Qad for each Qad
    public List<Qad> adjacencyList = new List<Qad>();
    //Qad in front (0)
    //Qad in back (1)
    //Qad in right (2)
    //Qad in left (3)


    //bfs variables 
    public bool visited = false;
    public Qad qParent = null;
    public int distance = 0;
    public bool corner = false;
    public bool ocupied = false;

    //layer
    public LayerMask layerQad;

    //Position inside Grid
    public bool row = false;
    public bool col = false;

    //hovering
    QadHoverScript hover;

    //Rigidbody
    Rigidbody qadRbd;

    //VFX
    [Header("VFX")]
    public Transform lightingPos;
    public bool lightingExists;
    public GameObject lightingStrikePrefab;
    public GameObject lightingStrike;
    public GameObject mainCamera;

    public GameObject attackParticleEffect;
    public GameObject attackParticleEffectPos;


    // Start is called before the first frame update
    void Start()
    {
        hover = GetComponent<QadHoverScript>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        qadDamage = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (current)
        {
            if (currentPieceType == 0) qadRender.material.color = Color.magenta;
            else qadRender.material.color = Color.gray;
        }
        else if (target) qadRender.material.color = Color.green;
        else if (selectable)
        {
            if (currentPieceType == 0) qadRender.material.color = Color.cyan;
            else qadRender.material.color = Color.yellow;

        }
        else if (attacked) qadRender.material.color = Color.red;
        else qadRender.material.color = Color.white;

        if (currentPiece != null)
        {
            if (currentPiece.gameObject.tag == "EnemyPiece" || currentPiece.gameObject.tag == "CurrentEnemyPiece")
            {
                currentPieceType = 1;
            }
            else if (currentPiece.gameObject.tag == "PlayerPiece" || currentPiece.gameObject.tag == "CurrentPlayerPiece")
            {
                currentPieceType = 0;
            }
        }
    }

    public void SetDamageInQad(float _damage)
    {
        qadDamage += _damage;
    }

    public void ResetDamage()
    {
        qadDamage = 0;
    }

    public void Reset()
    {
        walkable = true;
        current = false;
        target = false;
        selectable = false;
        attacked = false;
        diagonalToCurrent = false;

        visited = false;
        qParent = null;
        distance = 0;
        ocupied = false;

        currentPieceType = 0;


        adjacencyList.Clear();
    }

    //Pathfinding 
    public void FindNeighbors(float jumpHeight)
    {
        Reset();
        CheckQads(Vector3.forward, jumpHeight);
        CheckQads(-Vector3.forward, jumpHeight);
        CheckQads(Vector3.right, jumpHeight);
        CheckQads(-Vector3.right, jumpHeight);
    }


    public void CheckQads(Vector3 direction, float jumpHeight)
    {

        Vector3 halfExtents = new Vector3(0.25f, (1 + jumpHeight) / 2.0f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents, Quaternion.identity, layerQad);

        foreach (Collider col in colliders)
        {
            Qad qad = col.GetComponent<Qad>();

            if (qad != null)
            {
                CheckUp(qad.transform);
                RaycastHit hit;

                if (!Physics.Raycast(qad.transform.position, Vector3.up, out hit, 1))
                {
                    adjacencyList.Add(qad);
                }
                else
                {
                    if (hit.collider.gameObject != null)
                    {
                        if (hit.collider.gameObject.tag != "CurrentEnemyPiece" && hit.collider.gameObject.tag != "CurrentPlayerPiece")
                        {
                            if (hit.collider.gameObject.activeSelf == true)
                            {
                                walkable = false;

                                adjacencyList.Add(qad);
                            }
                        }
                    }
                }

            }
        }

    }

    public void CheckUp(Transform origin)
    {
        RaycastHit _hit;
        if (Physics.Raycast(origin.position, Vector3.up, out _hit, 1))
        {
            if (_hit.collider.gameObject != null)
            {
                if ((_hit.collider.gameObject.tag == "EnemyPiece" || _hit.collider.gameObject.tag == "PlayerPiece")
               && _hit.collider.gameObject.activeSelf == false)
                {
                    walkable = true;
                }
                else
                {
                    if (_hit.collider.gameObject == currentPiece)
                    {
                        ocupied = true;
                    }
                    walkable = false;

                }



            }

        }
        else
            walkable = true;
    }

    public void SetApartPiece()
    {
        if (currentPieceType == 0)//PlayerPiece
        {
            currentPiece.GetComponent<PlayerPieceControler>().SetApart();
            if (gameObject.GetComponent<Animator>().GetBool("RESET"))
            {
                gameObject.GetComponent<Animator>().SetBool("RESET", false);
            }
        }
        else//EnemyPiece
        {
            currentPiece.GetComponent<EnemyControler>().SetApart();
        }
    }

    public void LightingStrike()
    {

        if (!lightingExists)
        {
            lightingStrike = Instantiate(lightingStrikePrefab, lightingPos);
            lightingExists = true;
        }

        mainCamera.GetComponent<CameraControl>().Shake();
        mainCamera.GetComponent<CameraControl>();
    }

    public void SetAttackingParticleEffect(GameObject pE)
    {
        if (attackParticleEffect != null)
        {
            Destroy(attackParticleEffect);
        }
        attackParticleEffect = Instantiate(pE, attackParticleEffectPos.transform);
        attackParticleEffect.SetActive(false);
    }

    public void ActivateAttackingParticleEffect()
    {
        if (attackParticleEffect != null)
        {
            attackParticleEffect.SetActive(true);
        }
    }

    public void AddRigidBody()
    {

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Random.Range(0, 360), transform.eulerAngles.z);
        if (qadRbd == null)
        {
            gameObject.GetComponent<Animator>().enabled = false;
            qadRbd = gameObject.AddComponent<Rigidbody>();
            qadRbd.mass = 2f;
            qadRbd.AddForce(gameObject.transform.forward * 20f, ForceMode.Impulse);
        }
    }



}
