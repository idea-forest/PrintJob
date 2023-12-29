using System.Text;
using System.Threading.Tasks;

namespace PrintLoc.Model
{
    class Status
    {
        private static Status _instance;
        public static Status Instance => _instance ?? (_instance = new Status());

        public bool Online { get; set; }
    }
}
