using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PETR_Robot
{
    public static class Utils
    {
        public static T GetEnvironmentVariable<T>(string name)
        {
            string? value = Environment.GetEnvironmentVariable(name);
            if (value == null)
            {
                throw new InvalidOperationException($"{name} é necessário mas não foi encontrado.");
            }

            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (InvalidCastException)
            {
                throw new InvalidOperationException($"Não foi possível converter {name} para o tipo {typeof(T).Name}.");
            }
            catch (FormatException)
            {
                throw new InvalidOperationException($"{name} deve ser do tipo {typeof(T).Name} válido.");
            }
        }
    }
}
