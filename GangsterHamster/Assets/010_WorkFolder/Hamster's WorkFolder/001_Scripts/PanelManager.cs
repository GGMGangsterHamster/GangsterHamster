using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoSingleton<PanelManager>
{
    private Stack<Transform> pauseStack = new Stack<Transform>(); // 열고 닫는 창들을 Stack으로 관리

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 여기서 OpenPanel 함수를 통해서 어떤 창을 띄운다!!
        }
    }

    /// <summary>
    /// 이건 일시정지를 함과 동시에 
    /// 창을 띄우는 함수임니다 
    /// </summary>
    /// <param name="panel">띄울 패널의 Transform</param>
    public void OpenPanel(Transform panel)
    {
        if (panel.gameObject.activeSelf) // 이미 활성화 된 패널이라면 무시하기
        {
            return;
        }

        if (pauseStack.Count >= 1) // 이미 다른 창이 띄워져 있었더라면 그 창은 비활성화
        {
            pauseStack.Peek().gameObject.SetActive(false);
        }
        else
        {
            // 이곳에 게임을 퍼즈 시키는 코드 작성
        }

        panel.gameObject.SetActive(true); // 패널 활성화 시키기
        pauseStack.Push(panel); // 그리고 stack에 저장하기
    }

    public void ClosePanel()
    {
        if (pauseStack.Count >= 1) // stack의 길이가 1보다 클 경우에만 실행
        {
            pauseStack.Pop().gameObject.SetActive(false); // 지금 띄어져 있는 패널 비활성화

            if (pauseStack.Count != 0) // 한 패널을 끄고 난 후에 다른 패널이 남아있다면 그 패널을 활성화
            {
                pauseStack.Peek().gameObject.SetActive(true);
            }
            else
            {
                // 이곳에 게임이 퍼즈 된 상황을 푸는 코드 작성
            }
        }
    }

    public void AllClosePanel() // 모든 패널을 닫는 함수
    {
        while (pauseStack.Count >= 1) // 남은 패널이 없어질때까지 반복
        {
            pauseStack.Pop().gameObject.SetActive(false);

            if (pauseStack.Count == 0)
            {
                // 이곳에 게임이 퍼즈 된 상황을 푸는 코드 작성
            }
        }
    }
}
