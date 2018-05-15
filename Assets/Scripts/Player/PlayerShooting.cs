using DigitalRuby.PyroParticles;
using UnityEngine;
using UnityEngine.Networking;
using CSGO_DLV.Game;

namespace CSGO_DLV.NetworkPlayer
{
    public class PlayerShooting : NetworkBehaviour
    {
        [SerializeField]
        float shotCooldown = .3f;
        [SerializeField]
        Transform firePosition;
        [SerializeField]
        GameObject shotVfx;

        [SyncVar(hook = "OnScoreChanged")]
        int score = 0;

        private GameController gameController;
        float ellapsedTime;
        bool canShoot;

        void Start()
        {
            if (isLocalPlayer)
                canShoot = true;
            gameController = GameController.Controller;
        }

        void Update()
        {
            // In Game Menu
            if (isLocalPlayer && Input.GetKeyDown(KeyCode.Escape))
            {
                PlayerCanvas.canvas.ShowMenu(canShoot);
                canShoot = !canShoot;
            }

            if (!canShoot)
                return;

            ellapsedTime += Time.deltaTime;

            if (Input.GetButton("Fire1") && ellapsedTime > shotCooldown)
            {
                ellapsedTime = 0f;
                CmdFireShot(firePosition.position, firePosition.forward);
            }
        }

        [Command]
        void CmdFireShot(Vector3 origin, Vector3 direction)
        {
            GetComponentInChildren<Animator>().SetTrigger("shoot");

            // Ray cast for target
            RaycastHit hit;
            Ray ray = new Ray(origin, direction);
            Debug.DrawRay(ray.origin, ray.direction * 3f, Color.red, 1f);

            bool result = Physics.Raycast(ray, out hit, 50f);

            if (result)
            {
                PlayerHealth enemy = hit.transform.GetComponent<PlayerHealth>();

                if (enemy != null)
                {
                    bool wasKillShot = enemy.TakeDamage();
                    if (wasKillShot)
                        score = gameController.UpdateScore(score);
                }

            }
            RpcProcessShotEffects(result, hit.point);
        }

        void OnScoreChanged(int value)
        {
            score = value;
            if (isLocalPlayer)
            {
                PlayerCanvas.canvas.SetKills(value);
                // Maybe delete this
                if (gameController.GameWon(score))
                {
                    PlayerCanvas.canvas.WriteGameStatusText("You Won!");
                }
            }
        }

        [ClientRpc]
        void RpcProcessShotEffects(bool playImpact, Vector3 point)
        {
            Instantiate(shotVfx, firePosition);
        }

    }

}