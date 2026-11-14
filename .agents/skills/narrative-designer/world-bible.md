# World Bible — RELIC HAULERS (Biệt Đội Vớt Cổ Vật)

> Canonical reference for AI Agents writing `.ink` story scripts. Never invent conflicting geography, currencies, NPCs, or mechanics.

## 1. Cosmology & History
- **The Great Rift (*Đại Phân Tách*)**: Hundreds of years ago, the Ancient Council drew excessive abyssal energy through the **Sky-Anchors (*Lõi Từ Trường Neo Trời*)**. The continent shattered into drifting **Sky-Shards (*Quần Đảo Nổi*)** suspended above a cloud sea.
- **Collapse (*Đảo Sụp*)**: Islands gradually lose magnetic buoyancy during extraction; once the **Collapse Timer** hits `0:00`, the shard sinks into the Abyss (*Vực Thẳm*).
- **Vanguard Expedition (*Đội Tiên Phong*)**: The original crew sent down centuries ago. **VEX** is the fragmented consciousness copy of its Captain, embedded into the Sky-Dock radio & **Emergency Tether** network.

## 2. Factions & Entities
- **The Hauler Guild (*Hội Vớt Cổ Vật*)**: Official organization contracting Haulers at `01_SkyDockHub`. Secretly wants the final Core (`rel_void_crown`) to reboot the Ancient Machine.
- **Enemies (Corrupted Guardians)**: `Pebble Imp`, `Echo Stalker`, `Spring Mimic`, `Gust Wisp`, Mini-Boss `Clockwork Crusher`, and Final Boss `Golem Warden` (programmed by the Architects *not* to hate humans, but to stop anyone from pulling the final Anchor!).

## 3. Locations (6 Biomes + 2 Safe Zones)
- **Safe Zones**: `01_SkyDockHub` (*Trạm Chỉ Huy Khí Cầu* — Forge, Ranch, Aquarium, Museum, Drop Hatch), `02_AirshipShop` (*Khoang Tàu Nghỉ Giữa Chặng* — Pip's Shop, Cooking Galley).
- **Biome 1**: *Quần Đảo Gió Lộng* (`Windmill Meadow`, 1-1 / 1-2).
- **Biome 2**: *Rừng Nấm Phát Quang* (`Bioluminescent Canopy`, 2-1 / 2-2).
- **Biome 3**: *Xưởng Đúc Cổ Đại* (`Clockwork Foundry`, 3-1 / 3-2).
- **Biome 4**: *Vịnh Băng Pha Lê* (`Frostglass Glacier`, 4-1 / 4-2).
- **Biome 5**: *Đầm Lầy Trọng Lực* (`Abyssal Rift Swamp`, 5-1 / 5-2).
- **Biome 6**: *Thánh Điện Mây Vỡ* (`Shattered Spire Citadel`, 6-1 / 6-2).

## 4. Currencies & Key Items
- **Pod Value (`$`)**: Quota score inside a stage.
- **Surplus Credits (`¢`)**: 100% of `$` exceeding Quota, spent at Pip's `02_AirshipShop`.
- **Guild Tokens (`GT`)**: Meta-currency kept across runs (keep `50%` on Contract Failed) for Tech Tree & cosmetic equipment.
- **6 Ores (`OrePouch`)**: *Đồng Tầng Mây*, *Hổ Phách Phát Quang*, *Sắt Cổ Đại*, *Thạch Anh Băng*, *Tinh Thạch Hư Không*, *Khối Quặng Khổng Lồ* (`12kg`).
- **4 Ranch Species (`Sky-Ranch`)**: *Gà Mây Lông Bông*, *Cún Đá Imp Thuần Hóa*, *Cừu Pha Lê*, *Bọ Cạp Cơ Khí Mini*. Grown strictly per **Run Tick**, never real-time AFK.
- **Sky-Aquarium**: Breeds 12 Biome Fish species per 1–2 Run Ticks for Meal Buffs & *Vảy Cá Phát Sáng*; **strictly forbidden from Pod Quota**.
- **Emergency Tether Revive (*Tái Sinh Khẩn Cấp*)**: Pod uses a micro-pulse of Sky-Anchor memory to pull a fallen Hauler back to `LastSafeGroundPosition` (Solo: max 1–2/Run, costs `15s` Collapse Timer; disabled if `CollapseTimer <= 0`). Every tether pulse subtly drains VEX's stability, tying gameplay death into Act 2–3 narrative!
