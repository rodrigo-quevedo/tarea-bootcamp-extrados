using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configuration.FilesPathConfiguration
{
    public class FilesPathConfiguration : IFilesPathsConfigurations
    {
        private string IlustracionesPath {  get; set; }
        private string FotoPerfilPath { get; set; }

        public FilesPathConfiguration(string ilustraciones, string foto_perfil) 
        { 
            IlustracionesPath = ilustraciones;
            FotoPerfilPath = foto_perfil;
        }

        public string GetIlustracionesPath()
        {
            return this.IlustracionesPath;
        }

        public string GetFotoPerfilPath()
        {
            return this.FotoPerfilPath;
        }
    }
}
