using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blaved.Objects.Models.Configurations
{
    public class CryptographyConfiguration
    {
        public string Base64Key { get; init; } = default!;
        public string Base64IV { get; init; } = default!;
    }
}
