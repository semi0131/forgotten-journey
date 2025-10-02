using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
public enum PlayerState
{
    IDLE,
    MOVE,
    ATTACK,
    DAMAGED,
    DEBUFF,
    DEATH,
    OTHER,
}
public class SPUM_Prefabs : MonoBehaviour
{
    public float _version;
    public bool EditChk;
    public string _code;
    public Animator _anim;
    public AnimatorOverrideController OverrideController;

    public string UnitType;
    public List<SpumPackage> spumPackages = new List<SpumPackage>();
    public List<PreviewMatchingElement> ImageElement = new();
    public List<SPUM_AnimationData> SpumAnimationData = new();
    public Dictionary<string, List<AnimationClip>> StateAnimationPairs = new();
    public List<AnimationClip> IDLE_List = new();
    public List<AnimationClip> MOVE_List = new();
    public List<AnimationClip> ATTACK_List = new();
    public List<AnimationClip> DAMAGED_List = new();
    public List<AnimationClip> DEBUFF_List = new();
    public List<AnimationClip> DEATH_List = new();
    public List<AnimationClip> OTHER_List = new();

    // ⭐ Awake 함수 추가: 딕셔너리 초기화를 보장합니다.
    void Awake()
    {
        // PlayerMovement 스크립트가 호출되기 전에 딕셔너리와 컨트롤러를 초기화합니다.
        OverrideControllerInit();
        // 필요하다면 PopulateAnimationLists()도 여기서 호출하여 애니메이션 클립을 로드합니다.
        PopulateAnimationLists(); 
    }

    public void OverrideControllerInit()
    {
        Animator animator = _anim;
        // ⭐ 딕셔너리를 초기화하여 이전 오류(KeyNotFoundException)를 방지합니다.
        StateAnimationPairs = new Dictionary<string, List<AnimationClip>>();

        OverrideController = new AnimatorOverrideController();
        OverrideController.runtimeAnimatorController = animator.runtimeAnimatorController;

        // 모든 애니메이션 클립을 가져옵니다
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;

        foreach (AnimationClip clip in clips)
        {
            // 복제된 클립으로 오버라이드합니다
            OverrideController[clip.name] = clip;
        }

        animator.runtimeAnimatorController = OverrideController;
        foreach (PlayerState state in Enum.GetValues(typeof(PlayerState)))
        {
            var stateText = state.ToString();
            // ⭐ 딕셔너리에 모든 PlayerState 키를 빈 리스트와 함께 추가합니다.
            StateAnimationPairs[stateText] = new List<AnimationClip>();

            // 아래 switch 문에서 각 리스트를 해당 딕셔너리 값에 할당합니다.
            switch (stateText)
            {
                case "IDLE":
                    StateAnimationPairs[stateText] = IDLE_List;
                    break;
                case "MOVE":
                    StateAnimationPairs[stateText] = MOVE_List;
                    break;
                case "ATTACK":
                    StateAnimationPairs[stateText] = ATTACK_List;
                    break;
                case "DAMAGED":
                    StateAnimationPairs[stateText] = DAMAGED_List;
                    break;
                case "DEBUFF":
                    StateAnimationPairs[stateText] = DEBUFF_List;
                    break;
                case "DEATH":
                    StateAnimationPairs[stateText] = DEATH_List;
                    break;
                case "OTHER":
                    StateAnimationPairs[stateText] = OTHER_List;
                    break;
            }
        }
    }
    public bool allListsHaveItemsExist()
    {
        List<List<AnimationClip>> allLists = new List<List<AnimationClip>>()
        {
            IDLE_List, MOVE_List, ATTACK_List, DAMAGED_List, DEBUFF_List, DEATH_List, OTHER_List
        };

        return allLists.All(list => list.Count > 0);
    }
    [ContextMenu("PopulateAnimationLists")]
    public void PopulateAnimationLists()
    {
        IDLE_List = new();
        MOVE_List = new();
        ATTACK_List = new();
        DAMAGED_List = new();
        DEBUFF_List = new();
        DEATH_List = new();
        OTHER_List = new();

        var groupedClips = spumPackages
        .SelectMany(package => package.SpumAnimationData)
        .Where(spumClip => spumClip.HasData &&
                             spumClip.UnitType.Equals(UnitType) &&
                             spumClip.index > -1)
        .GroupBy(spumClip => spumClip.StateType)
        .ToDictionary(
            group => group.Key,
            group => group.OrderBy(clip => clip.index).ToList()
        );
        // foreach (var item in groupedClips)
        // {
        //      foreach (var clip in item.Value)
        //      {
        //          Debug.Log(clip.ClipPath);
        //      }
        // }
        foreach (var kvp in groupedClips)
        {
            var stateType = kvp.Key;
            var orderedClips = kvp.Value;
            switch (stateType)
            {
                case "IDLE":
                    IDLE_List.AddRange(orderedClips.Select(clip => LoadAnimationClip(clip.ClipPath)));
                    //StateAnimationPairs[stateType] = IDLE_List;
                    break;
                case "MOVE":
                    MOVE_List.AddRange(orderedClips.Select(clip => LoadAnimationClip(clip.ClipPath)));
                    //StateAnimationPairs[stateType] = MOVE_List;
                    break;
                case "ATTACK":
                    ATTACK_List.AddRange(orderedClips.Select(clip => LoadAnimationClip(clip.ClipPath)));
                    //StateAnimationPairs[stateType] = ATTACK_List;
                    break;
                case "DAMAGED":
                    DAMAGED_List.AddRange(orderedClips.Select(clip => LoadAnimationClip(clip.ClipPath)));
                    //StateAnimationPairs[stateType] = DAMAGED_List;
                    break;
                case "DEBUFF":
                    DEBUFF_List.AddRange(orderedClips.Select(clip => LoadAnimationClip(clip.ClipPath)));
                    //StateAnimationPairs[stateType] = DEBUFF_List;
                    break;
                case "DEATH":
                    DEATH_List.AddRange(orderedClips.Select(clip => LoadAnimationClip(clip.ClipPath)));
                    //StateAnimationPairs[stateType] = DEATH_List;
                    break;
                case "OTHER":
                    OTHER_List.AddRange(orderedClips.Select(clip => LoadAnimationClip(clip.ClipPath)));
                    //StateAnimationPairs[stateType] = OTHER_List;
                    break;
            }
        }

    }
    public void PlayAnimation(PlayerState PlayState, int index)
    {
        Animator animator = _anim;
        //Debug.Log(PlayState.ToString());
        var animations = StateAnimationPairs[PlayState.ToString()];
        //Debug.Log(OverrideController[PlayState.ToString()].name);
        OverrideController[PlayState.ToString()] = animations[index];
        //Debug.Log( OverrideController[PlayState.ToString()].name);
        var StateStr = PlayState.ToString();

        bool isMove = StateStr.Contains("MOVE");
        bool isDebuff = StateStr.Contains("DEBUFF");
        bool isDeath = StateStr.Contains("DEATH");
        animator.SetBool("1_Move", isMove);
        animator.SetBool("5_Debuff", isDebuff);
        animator.SetBool("isDeath", isDeath);
        if (!isMove && !isDebuff)
        {
            AnimatorControllerParameter[] parameters = animator.parameters;
            foreach (AnimatorControllerParameter parameter in parameters)
            {
                // if(parameter.type == AnimatorControllerParameterType.Bool){
                //      bool isBool = StateStr.ToUpper().Contains(parameter.name.ToUpper());
                //      animator.SetBool(parameter.name, isBool);
                // }
                if (parameter.type == AnimatorControllerParameterType.Trigger)
                {
                    bool isTrigger = parameter.name.ToUpper().Contains(StateStr.ToUpper());
                    if (isTrigger)
                    {
                        Debug.Log($"Parameter: {parameter.name}, Type: {parameter.type}");
                        animator.SetTrigger(parameter.name);
                    }
                }
            }
        }
    }
    AnimationClip LoadAnimationClip(string clipPath)
    {
        // "Animations" 폴더에서 애니메이션 클립 로드
        AnimationClip clip = Resources.Load<AnimationClip>(clipPath.Replace(".anim", ""));

        if (clip == null)
        {
            Debug.LogWarning($"Failed to load animation clip '{clipPath}'.");
        }

        return clip;
    }
}
