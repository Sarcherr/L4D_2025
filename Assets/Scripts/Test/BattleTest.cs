using UnityEngine;

public class BattleTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //List<string> names = new List<string> { "Dolora", "Nora", "Jia_Baoyu" };
        BattleManager.Instance.InitBattle(CharacterSelectManager.Instance.SelectedCharacters,
            LevelSelectManager.Instance.MonsterNames);
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
