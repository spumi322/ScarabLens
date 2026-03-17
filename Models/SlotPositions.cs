namespace ScarabLens;

public static class SlotPositions
{
    public static readonly List<(string Name, double X, double Y)> All = new()
    {
        // Left group
        ("Cartography Scarab of Escalation", 75, 205), ("Cartography Scarab of Risk", 115, 205), ("Cartography Scarab of the Multitude", 155, 205), ("Cartography Scarab of Corruption", 195, 205),
        ("Divination Scarab of The Cloister", 75, 255), ("Divination Scarab of Plenty", 115, 255), ("Divination Scarab of Pilfering", 155, 255),
        ("Bestiary Scarab", 75, 300), ("Bestiary Scarab of the Herd", 115, 300), ("Bestiary Scarab of Duplicating", 155, 300),
        ("Betrayal Scarab", 75, 350), ("Betrayal Scarab of the Allflame", 115, 350), ("Betrayal Scarab of Reinforcements", 155, 350), ("Betrayal Scarab of Unbreaking", 195, 350),
        ("Incursion Scarab", 75, 395), ("Incursion Scarab of Invasion", 115, 395), ("Incursion Scarab of Champions", 155, 395), ("Incursion Scarab of Timelines", 195, 395),
        ("Sulphite Scarab", 75, 445), ("Sulphite Scarab of Fumes", 115, 445),
        ("Ambush Scarab", 75, 490), ("Ambush Scarab of Hidden Compartments", 115, 490), ("Ambush Scarab of Potency", 155, 490), ("Ambush Scarab of Containment", 200, 490), ("Ambush Scarab of Discernment", 240, 490),
        ("Anarchy Scarab", 75, 535), ("Anarchy Scarab of Gigantification", 115, 535), ("Anarchy Scarab of Partnership", 155, 535), ("Anarchy Scarab of the Exceptional", 195, 535),
        ("Beyond Scarab", 75, 585), ("Beyond Scarab of Haemophilia", 115, 585), ("Beyond Scarab of Resurgence", 155, 585), ("Beyond Scarab of the Invasion", 195, 585),
        ("Domination Scarab", 75, 632), ("Domination Scarab of Apparitions", 115, 632), ("Domination Scarab of Evolution", 155, 632), ("Domination Scarab of Terrors", 195, 632),
        ("Essence Scarab", 75, 680), ("Essence Scarab of Ascent", 115, 680), ("Essence Scarab of Stability", 155, 680), ("Essence Scarab of Calcification", 195, 680), ("Essence Scarab of Adaptation", 240, 680),
        ("Torment Scarab", 75, 725), ("Torment Scarab of Peculiarity", 115, 725), ("Torment Scarab of Possession", 155, 725),

        // Middle group
        ("Influencing Scarab of the Shaper", 320, 205), ("Influencing Scarab of the Elder", 360, 205), ("Influencing Scarab of Hordes", 400, 205), ("Influencing Scarab of Interference", 440, 205),
        ("Titanic Scarab", 320, 255), ("Titanic Scarab of Treasures", 360, 255), ("Titanic Scarab of Legend", 400, 255),
        ("Abyss Scarab", 320, 300), ("Abyss Scarab of Multitudes", 360, 300), ("Abyss Scarab of Edifice", 400, 300), ("Abyss Scarab of Descending", 440, 300), ("Abyss Scarab of Profound Depth", 480, 300),
        ("Blight Scarab", 320, 350), ("Blight Scarab of the Blightheart", 360, 350), ("Blight Scarab of Blooming", 400, 350), ("Blight Scarab of Invigoration", 440, 350),
        ("Breach Scarab of the Hive", 320, 395), ("Breach Scarab of Instability", 360, 395), ("Breach Scarab of the Marshal", 400, 395), ("Breach Scarab of the Incensed Swarm", 440, 395), ("Breach Scarab of Resonant Cascade", 480, 395),
        ("Delirium Scarab", 320, 445), ("Delirium Scarab of Mania", 360, 445), ("Delirium Scarab of Paranoia", 400, 445), ("Delirium Scarab of Neuroses", 440, 445), ("Delirium Scarab of Delusions", 480, 445),
        ("Expedition Scarab", 320, 490), ("Expedition Scarab of Runefinding", 360, 490), ("Expedition Scarab of Verisium Powder", 400, 490), ("Expedition Scarab of Infusion", 440, 490), ("Expedition Scarab of Archaeology", 480, 490),
        ("Harvest Scarab", 320, 535), ("Harvest Scarab of Doubling", 360, 535), ("Harvest Scarab of Cornucopia", 400, 535),
        ("Kalguuran Scarab", 320, 585), ("Kalguuran Scarab of Guarded Riches", 360, 585), ("Kalguuran Scarab of Refinement", 400, 585), ("Kalguuran Scarab of Enriching", 440, 585),
        ("Legion Scarab", 320, 632), ("Legion Scarab of Officers", 360, 632), ("Legion Scarab of Treasures", 400, 632), ("Legion Scarab of Eternal Conflict", 440, 632),
        ("Ritual Scarab of Selectiveness", 320, 680), ("Ritual Scarab of Wisps", 360, 680), ("Ritual Scarab of Abundance", 400, 680), ("Ritual Scarab of Corpses", 440, 680),
        ("Ultimatum Scarab", 320, 725), ("Ultimatum Scarab of Bribing", 360, 725), ("Ultimatum Scarab of Dueling", 400, 725), ("Ultimatum Scarab of Catalysing", 440, 725), ("Ultimatum Scarab of Inscription", 480, 725),

        // Right group
        ("Scarab of Monstrous Lineage", 565, 325),
        ("Horned Scarab of Bloodlines", 605, 345),
        ("Scarab of Adversaries", 565, 365),
        ("Horned Scarab of Nemeses", 605, 385),
        ("Scarab of Divinity", 565, 405),
        ("Horned Scarab of Preservation", 605, 425),
        ("Scarab of the Dextral", 565, 445),
        ("Horned Scarab of Awakening", 605, 465),
        ("Scarab of the Sinistral", 565, 485),
        ("Horned Scarab of Tradition", 605, 505),
        ("Scarab of Wisps", 565, 525),
        ("Horned Scarab of Glittering", 605, 545),
        ("Scarab of Radiant Storms", 565, 565),
        ("Horned Scarab of Pandemonium", 605, 585),
        ("Scarab of Stability", 565, 605),
    };
}