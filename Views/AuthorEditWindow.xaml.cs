using LibraryManagementSystem.ViewModels;
using System.Windows;

namespace LibraryManagementSystem.Views
{
    public partial class AuthorEditWindow : Window
    {
        public AuthorEditWindow()
        {
            InitializeComponent();
        }
        
        public AuthorEditWindow(AuthorEditViewModel viewModel) : this()
        { 
            DataContext = viewModel;
            viewModel.RequestClose += () => DialogResult = true;
        }
    }
}