using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoSingleton<PanelManager>
{
    private Stack<Transform> pauseStack = new Stack<Transform>(); // ���� �ݴ� â���� Stack���� ����

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // ���⼭ OpenPanel �Լ��� ���ؼ� � â�� ����!!
        }
    }

    /// <summary>
    /// �̰� �Ͻ������� �԰� ���ÿ� 
    /// â�� ���� �Լ��Ӵϴ� 
    /// </summary>
    /// <param name="panel">��� �г��� Transform</param>
    public void OpenPanel(Transform panel)
    {
        if (panel.gameObject.activeSelf) // �̹� Ȱ��ȭ �� �г��̶�� �����ϱ�
        {
            return;
        }

        if (pauseStack.Count >= 1) // �̹� �ٸ� â�� ����� �־������ �� â�� ��Ȱ��ȭ
        {
            pauseStack.Peek().gameObject.SetActive(false);
        }
        else
        {
            // �̰��� ������ ���� ��Ű�� �ڵ� �ۼ�
        }

        panel.gameObject.SetActive(true); // �г� Ȱ��ȭ ��Ű��
        pauseStack.Push(panel); // �׸��� stack�� �����ϱ�
    }

    public void ClosePanel()
    {
        if (pauseStack.Count >= 1) // stack�� ���̰� 1���� Ŭ ��쿡�� ����
        {
            pauseStack.Pop().gameObject.SetActive(false); // ���� ����� �ִ� �г� ��Ȱ��ȭ

            if (pauseStack.Count != 0) // �� �г��� ���� �� �Ŀ� �ٸ� �г��� �����ִٸ� �� �г��� Ȱ��ȭ
            {
                pauseStack.Peek().gameObject.SetActive(true);
            }
            else
            {
                // �̰��� ������ ���� �� ��Ȳ�� Ǫ�� �ڵ� �ۼ�
            }
        }
    }

    public void AllClosePanel() // ��� �г��� �ݴ� �Լ�
    {
        while (pauseStack.Count >= 1) // ���� �г��� ������������ �ݺ�
        {
            pauseStack.Pop().gameObject.SetActive(false);

            if (pauseStack.Count == 0)
            {
                // �̰��� ������ ���� �� ��Ȳ�� Ǫ�� �ڵ� �ۼ�
            }
        }
    }
}
