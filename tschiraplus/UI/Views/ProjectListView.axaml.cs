using Avalonia.ReactiveUI;
using UI.ViewModels;

namespace UI.Views;

public partial class ProjectListView : ReactiveUserControl<ProjectListViewModel>
{
    public ProjectListView()
    {
        InitializeComponent();
    }
}