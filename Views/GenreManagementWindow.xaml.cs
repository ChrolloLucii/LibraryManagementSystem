using LibraryManagementSystem.ViewModels;
using System.Windows;

namespace LibraryManagementSystem.Views
{
    public partial class GenreManagementWindow : Window
    {
        public GenreManagementWindow(GenreManagementViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}