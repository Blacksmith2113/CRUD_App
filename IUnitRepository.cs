using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_App {
    interface IUnitRepository {
        bool Create(Unit e);
        ObservableCollection<Unit> ReadAll();
        Unit Read(int id);
        bool Edit(Unit e);
        bool Delete(Unit e);
    }
}
