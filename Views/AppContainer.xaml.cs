namespace Cohabitation.Views;

public partial class AppContainer : TabbedPage
{
	public AppContainer()
	{
        try
        {
            InitializeComponent();
        }
        catch (System.Reflection.TargetInvocationException ex)
        {
            // ������O�̏ڍׂ����O�ɏo��
            Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
            Console.WriteLine($"Stack Trace: {ex.InnerException?.StackTrace}");
            throw; // ��O���ăX���[���āA�Ăяo�����ŃL���b�`�ł���悤�ɂ���
        }
    }
}