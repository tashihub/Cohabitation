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
            // 内部例外の詳細をログに出力
            Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
            Console.WriteLine($"Stack Trace: {ex.InnerException?.StackTrace}");
            throw; // 例外を再スローして、呼び出し元でキャッチできるようにする
        }
    }
}