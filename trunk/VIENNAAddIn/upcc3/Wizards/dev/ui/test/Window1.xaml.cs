using System.Collections.ObjectModel;
namespace VIENNAAddIn.upcc3.Wizards.dev.ui.test
{
    public partial class Window1 : System.Windows.Window
    {

        static public ObservableCollection<AnimalCategory> AnimalCategories

            = new ObservableCollection<AnimalCategory>();



        public Window1()
        {

            InitializeComponent();



            ObservableCollection<Animal> animals = new ObservableCollection<Animal>();

            animals.Add(new Animal("California Newt"));

            animals.Add(new Animal("Tomato Frog"));

            animals.Add(new Animal("Green Tree Frog"));

            AnimalCategories.Add(new AnimalCategory("Amphibians", animals));



            animals = new ObservableCollection<Animal>();

            animals.Add(new Animal("Golden Silk Spider"));

            animals.Add(new Animal("Black Widow Spider"));

            AnimalCategories.Add(new AnimalCategory("Spiders", animals));

        }

    } 
}
