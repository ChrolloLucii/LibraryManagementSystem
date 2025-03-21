using LibraryManagementSystem.ViewModels;
using System.Windows;

namespace LibraryManagementSystem.Views
{
    public partial class AuthorManagementWindow : Window
    {
        public AuthorManagementWindow(AuthorManagementViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}