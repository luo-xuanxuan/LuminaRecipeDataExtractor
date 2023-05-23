using LuminaRecipeDataExtractor;
using Lumina.Excel.GeneratedSheets;
using Newtonsoft.Json;

var lumina = new Lumina.GameData("C:/Program Files (x86)/SquareEnix/FINAL FANTASY XIV - A Realm Reborn/game/sqpack");

lumina.Options.PanicOnSheetChecksumMismatch = false;

var itemTable = lumina.GetExcelSheet<Item>();
var recipeLookupTable = lumina.GetExcelSheet<RecipeLookup>();

List<RecipeData> data = new List<RecipeData>();

bool ProcessRecipe(string job, Recipe value)
{
    if (value.ToString() == "Recipe#0")
        return false;

    foreach (var recipeData in data)
    {
        if (recipeData.name == value.ItemResult.Value.Name.ToString())
        {
            recipeData.jobs.Add(job);
            return true;
        }
    }

    RecipeData recipe = new RecipeData();
    recipe.name = value.ItemResult.Value.Name.ToString();
    recipe.jobs.Add(job);
    recipe.job_level = value.RecipeLevelTable.Value.ClassJobLevel;
    recipe.recipe_level = (int)value.RecipeLevelTable.Value.RowId;
    recipe.item_level = (int)value.ItemResult.Value.LevelItem.Row;
    recipe.equip_level = value.ItemResult.Value.LevelEquip;
    recipe.stars = value.RecipeLevelTable.Value.Stars;
    recipe.progress = (value.DifficultyFactor * value.RecipeLevelTable.Value.Difficulty) / 100;
    recipe.quality = (int)((value.QualityFactor * value.RecipeLevelTable.Value.Quality) / 100);
    recipe.durability = (int)((value.DurabilityFactor * value.RecipeLevelTable.Value.Durability) / 100);
    recipe.progress_div = value.RecipeLevelTable.Value.ProgressDivider;
    recipe.progress_mod = value.RecipeLevelTable.Value.ProgressModifier;
    recipe.quality_div = value.RecipeLevelTable.Value.QualityDivider;
    recipe.quality_mod = value.RecipeLevelTable.Value.QualityModifier;
    recipe.is_specialist = value.IsSpecializationRequired;
    recipe.is_expert = value.IsExpert;
    recipe.conditions_flag = value.RecipeLevelTable.Value.ConditionsFlag;
    recipe.can_hq = value.CanHq;
    recipe.material_quality = value.MaterialQualityFactor;
    for (int i = 0; i < 10; i++)
    {
        if (value.UnkData5[i].ItemIngredient == -1)
            continue;
        if (itemTable.GetRow((uint)value.UnkData5[i].ItemIngredient).ToString() == "Item#0")
            continue;
        Ingredient ing = new Ingredient();
        ing.name = itemTable.GetRow((uint)value.UnkData5[i].ItemIngredient).Name;
        ing.amount = value.UnkData5[i].AmountIngredient;
        ing.can_hq = itemTable.GetRow((uint)value.UnkData5[i].ItemIngredient).CanBeHq;
        ing.item_level = (int)itemTable.GetRow((uint)value.UnkData5[i].ItemIngredient).LevelItem.Row;
        recipe.ingredients.Add(ing);
    }
    data.Add(recipe);
    return true;
}

foreach (var row in recipeLookupTable)
{
    ProcessRecipe("CRP", row.CRP.Value);
    ProcessRecipe("ARM", row.ARM.Value);
    ProcessRecipe("BSM", row.BSM.Value);
    ProcessRecipe("ALC", row.ALC.Value);
    ProcessRecipe("GSM", row.GSM.Value);
    ProcessRecipe("CUL", row.CUL.Value);
    ProcessRecipe("WVR", row.WVR.Value);
    ProcessRecipe("LTW", row.LTW.Value);
}
using (StreamWriter writer = new StreamWriter("./recipes.json"))
{
    var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
    writer.Write(JsonConvert.SerializeObject(data, settings));
}