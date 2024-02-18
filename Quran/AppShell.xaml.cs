using Quran.Views;

namespace Quran;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(VersesPage), typeof(VersesPage));
		Routing.RegisterRoute(nameof(ChaptersPage), typeof(ChaptersPage));
	}
}
