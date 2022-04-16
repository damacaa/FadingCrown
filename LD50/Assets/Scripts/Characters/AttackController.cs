using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    CharacterController character;

    [SerializeField]
    LayerMask targetLayerMask;
    protected SoundManager audios;
    protected AudioSource selfAudio;

    [SerializeField]
    int damage;
    [SerializeField]
    int kockback;
    [SerializeField]
    int dashDamage;
    [SerializeField]
    int dashKnockback;

    bool canAttack = true;

    [SerializeField]
    Transform target;
    [SerializeField]
    float attackSize = 1f;
    [SerializeField]
    float attackRange = 1f;
    [SerializeField]
    float reloadTime = 1f;
    float lastTime = 0;

    bool flip;

    [SerializeField]
    GameObject attackPrefab;


    private void Awake()
    {
        character = GetComponent<CharacterController>();
        audios = GameObject.FindObjectOfType<SoundManager>().GetComponent<SoundManager>();
        selfAudio = GetComponent<AudioSource>();

        canAttack = !GameManager.achivedFlowers.Contains(FlowerTypes.ATTACK);
    }

    public void Attack()
    {
        if (!canAttack) return;

        if (Time.time < lastTime + reloadTime)
            return;
        audios.AudioSelect(SoundManager.audioType.Slash, selfAudio);
        print(name + " attacks");
        lastTime = Time.time;

        RaycastHit2D[] hits = Physics2D.CircleCastAll(target.position, attackSize, Vector2.right * character.direction, attackRange, targetLayerMask);
        Debug.DrawRay(target.position, Vector2.right * character.direction * attackRange, Color.white, 1f);

        AnimatedEffect slashEffect = GameObject.Instantiate(attackPrefab, transform.position + Vector3.right * character.direction, Quaternion.identity).GetComponent<AnimatedEffect>();
        slashEffect.user = GetComponent<CharacterController>();
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.TryGetComponent<CharacterController>(out CharacterController character) && character != GetComponent<CharacterController>())
            {
                if (!slashEffect.targets.Contains(character))
                    slashEffect.targets.Add(character);
            }
        }

        audios.AudioSelect(SoundManager.audioType.SlashHit, selfAudio);

        if (character.direction < 0)
            slashEffect.Flip();

        slashEffect.transform.parent = transform;
    }

    private void Update()
    {
        if (!target)
        {
            GameObject newTarget = new GameObject("Target");
            newTarget.transform.parent = transform;
            newTarget.transform.localPosition = Vector2.zero;
            target = newTarget.transform;
        }
    }
}
