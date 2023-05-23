namespace LuminaRecipeDataExtractor
{
    public class Ingredient
    {
        public string name { get; set; }
        public int amount { get; set; }
        public int item_level { get; set; }
        public bool can_hq { get; set; }
    }

    public class RecipeData
    {
        public string name { get; set; }
        public List<string> jobs;
        public int job_level { get; set; }
        public int recipe_level { get; set; }
        public int item_level { get; set; }
        public int equip_level { get; set; }
        public int stars { get; set; }
        public int progress { get; set; }
        public int quality { get; set; }
        public int durability { get; set; }
        public int progress_div { get; set; }
        public int progress_mod { get; set; }
        public int quality_div { get; set; }
        public int quality_mod { get; set; }
        public bool is_specialist { get; set; }
        public bool is_expert { get; set; }
        public int conditions_flag { get; set; }
        public bool can_hq { get; set; }
        public int material_quality { get; set; }
        public List<Ingredient> ingredients;

        public RecipeData()
        {
            jobs = new List<string>();
            ingredients = new List<Ingredient>();
        }
    }
}
