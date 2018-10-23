using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemmoAppBar.Emtity
{
    class Photos
    {
        private string _fileName;
        private string _filePath;

        public string fileName { get => _fileName; set => _fileName = value; }
        public string filePath { get => _filePath; set => _filePath = value; }
    }
}
