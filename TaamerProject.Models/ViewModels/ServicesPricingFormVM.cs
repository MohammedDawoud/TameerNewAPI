using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models
{
    public class ServicesPricingFormVM
    {
        public int FormId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? LandArea { get; set; }
        public string? NumofStreetsAdjacent { get; set; }
        public string? Height { get; set; }
        public string? Width { get; set; }
        public int? FormType { get; set; }
        public string? ServiceNotes { get; set; }
        public bool? Bas_swimmingPool { get; set; }
        public bool? Bas_board { get; set; }
        public bool? Bas_garden { get; set; }
        public bool? Bas_LaundryRoom { get; set; }
        public bool? Bas_storehouse { get; set; }
        public bool? Bas_carparking { get; set; }
        public bool? Bas_Desk { get; set; }
        public bool? Bas_openkitchen { get; set; }
        public bool? Bas_closedkitchen { get; set; }
        public bool? Bas_bedroomonly { get; set; }
        public bool? Bas_Bedandbathroom { get; set; }
        public bool? Bas_Healthclub { get; set; }
        public bool? Bas_Multipurposehall { get; set; }
        public bool? Bas_Kidsplayroom { get; set; }
        public bool? Bas_Homecinemahall { get; set; }
        public string? Bas_Notes { get; set; }
        public bool? Gro_familyliving { get; set; }
        public bool? Gro_guestdiningroom { get; set; }
        public bool? Gro_guestcouncil { get; set; }
        public bool? Gro_closedkitchen { get; set; }
        public bool? Gro_Desk { get; set; }
        public bool? Gro_familydiningroom { get; set; }
        public bool? Gro_elevator { get; set; }
        public bool? Gro_Store { get; set; }
        public bool? Gro_openkitchen { get; set; }
        public bool? Gro_SubEntrance { get; set; }
        public bool? Gro_MainEntrance { get; set; }
        public bool? Gro_Toilets { get; set; }
        public bool? Gro_Extrabedandbathroom { get; set; }
        public bool? Gro_servicedrawer { get; set; }
        public bool? Gro_maindrawer { get; set; }
        public bool? Gro_Laundryandironingroom { get; set; }
        public bool? Gro_Maidsroomandbathroom { get; set; }
        public bool? Gro_Extrabedroomonly { get; set; }
        public string? Gro_Numberofguestboards { get; set; }
        public string? Gro_Numberoftoilets { get; set; }
        public string? Gro_Notes { get; set; }
        public bool? Firrou_bedroomonly { get; set; }
        public bool? Firrou_bedandbathroom { get; set; }
        public bool? Firrou_Masterbedroomsuite { get; set; }
        public bool? Firrou_Smallandopenkitchen { get; set; }
        public bool? Firrou_familysitting { get; set; }
        public bool? Firrou_WC { get; set; }
        public bool? Firrou_Gym { get; set; }
        public bool? Firrou_serviceroom { get; set; }
        public bool? Firrou_Desk { get; set; }
        public string? Firrou_Numofbednadbathroom { get; set; }
        public string? Firrou_Numofbedroomonly { get; set; }
        public string? Firrou_Notes { get; set; }
        public bool? Sur_storehouse { get; set; }
        public bool? Sur_WC { get; set; }
        public bool? Sur_surface { get; set; }
        public bool? Sur_Maidsbedroom { get; set; }
        public bool? Sur_Laundryandironingroom { get; set; }
        public bool? Sur_multiusehall { get; set; }
        public string? Sur_Notes { get; set; }
        public bool? Gar_waterbodyonly { get; set; }
        public bool? Gar_Externalstaircase { get; set; }
        public bool? Gar_Childrensplayarea { get; set; }
        public bool? Gar_outdoorsitting { get; set; }
        public bool? Gar_swimmingpool { get; set; }
        public bool? Ext_Council { get; set; }
        public bool? Ext_Multipurposehall { get; set; }
        public bool? Ext_WC { get; set; }
        public bool? Par_OneCar { get; set; }
        public bool? Par_TwoCar { get; set; }
        public bool? Par_MoreCars { get; set; }
        public string? Par_NoofCars { get; set; }
        public bool? Dri_OneDriversroomtoilet { get; set; }
        public bool? Dri_TwoDriversroomwithtoilets { get; set; }
        public bool? Dri_MoreDriversroomwithtoilets { get; set; }
        public string? Dri_NoofDriversroomwithtoilets { get; set; }
        public string? AnotherNotes { get; set; }
        public bool? FormStatus { get; set; }

        public string? Date { get; set; }
        public string? URLFile { get; set; }
        public int? BranchId { get; set; }


    }
}
