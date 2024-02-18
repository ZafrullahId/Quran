using Quran.Domain;
using Quran.ViewModels;

namespace Quran.Views;

public partial class ChaptersPage : ContentPage
{
    private readonly ChaptersViewModel chaptersViewModel;
    public ChaptersPage(ChaptersViewModel chaptersViewModel)
    {
        InitializeComponent();
        this.chaptersViewModel = chaptersViewModel;
        this.BindingContext = chaptersViewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await this.chaptersViewModel.LoadChapters();
    }
    async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ChapterCollection.SelectedItem == null)
        {
            Chapter previous = (e.PreviousSelection.FirstOrDefault() as Chapter);
            await this.chaptersViewModel.GotToChaperVerses(previous);
        }
        else
        {
            Chapter current = (e.CurrentSelection.FirstOrDefault() as Chapter);
            ChapterCollection.SelectedItem = null;
        }
    }


}