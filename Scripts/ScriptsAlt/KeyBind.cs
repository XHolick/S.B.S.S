using UnityEngine;
using UnityEngine.UI;

public class KeyBindingManager : MonoBehaviour
{
    public PlayerMovement player1Movement;  // Referência para o script de movimento do Player 1
    public PlayerMovement player2Movement;  // Referência para o script de movimento do Player 2

    public Button remapPlayer1Button;  // Botão para remapear as teclas do Player 1
    public Button remapPlayer2Button;  // Botão para remapear as teclas do Player 2

    private PlayerMovement currentPlayer;  // Jogador atual sendo configurado

    void Start()
    {
        remapPlayer1Button.onClick.AddListener(() => StartRemapping(player1Movement));
        remapPlayer2Button.onClick.AddListener(() => StartRemapping(player2Movement));
    }

    void StartRemapping(PlayerMovement player)
    {
        currentPlayer = player;  // Define qual jogador está sendo configurado
        Debug.Log("Iniciando remapeamento de teclas...");
    }

    void Update()
    {
        if (currentPlayer != null)
        {
            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    currentPlayer.upKey = Input.inputString.ToUpper() == "W" ? KeyCode.W : KeyCode.UpArrow;
                    Debug.Log("Tecla 'Para Cima' remapeada.");
                }
                else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                {
                    currentPlayer.downKey = Input.inputString.ToUpper() == "S" ? KeyCode.S : KeyCode.DownArrow;
                    Debug.Log("Tecla 'Para Baixo' remapeada.");
                }
                else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    currentPlayer.leftKey = Input.inputString.ToUpper() == "A" ? KeyCode.A : KeyCode.LeftArrow;
                    Debug.Log("Tecla 'Esquerda' remapeada.");
                }
                else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    currentPlayer.rightKey = Input.inputString.ToUpper() == "D" ? KeyCode.D : KeyCode.RightArrow;
                    Debug.Log("Tecla 'Direita' remapeada.");
                }

                // Finaliza o remapeamento
                currentPlayer = null;
                Debug.Log("Remapeamento concluído.");
            }
        }
    }
}
