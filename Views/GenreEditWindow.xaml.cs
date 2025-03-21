using LibraryManagementSystem.ViewModels;
using System.Windows;

namespace LibraryManagementSystem.Views
{
    public partial class GenreEditWindow : Window
    {
        public GenreEditWindow()
        {
            InitializeComponent();
        }
        
        public GenreEditWindow(GenreEditViewModel viewModel) : this()
        { 
            DataContext = viewModel;
            viewModel.RequestClose += () => DialogResult = true;
        }
    }
}