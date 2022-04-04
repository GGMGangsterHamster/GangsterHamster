interface IUIAction
{
    /// <summary>
    /// ó�� ������ ���� �Ǿ��� �� �ѹ��� ȣ���ϴ� �Լ�
    /// </summary>
    public void InitActions();

    /// <summary>
    /// �ش��ϴ� �г��� Ȱ��ȭ �ɶ����� ȣ���ϴ� �Լ�
    /// </summary>
    public void ActivationActions();

    /// <summary>
    /// �ش��ϴ� �г��� ��Ȱ��ȭ �ɶ����� ȣ���ϴ� �Լ�
    /// </summary>
    public void DeActivationActions();
}
