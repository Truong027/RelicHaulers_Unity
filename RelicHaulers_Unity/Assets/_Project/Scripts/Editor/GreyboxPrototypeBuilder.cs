#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using RelicHaulers.Core;
using RelicHaulers.Player;
using RelicHaulers.Relics;
using RelicHaulers.UI;

namespace RelicHaulers.EditorTools
{
    /// <summary>
    /// One-click menu builder for Gate 1 Prototype Scenes (Slice 1A Movement + Slice 1B Carry/Throw & Chest).
    /// </summary>
    public static class GreyboxPrototypeBuilder
    {
        private const string ScenesFolder = "Assets/_Project/Scenes";
        private const string MaterialsFolder = "Assets/_Project/Materials";

        [MenuItem("RelicHaulers/Gate 1/Build Prototype Scenes (Slice 1A + 1B)")]
        public static void BuildPrototypeScenes()
        {
            EnsureFolder("Assets/_Project");
            EnsureFolder(ScenesFolder);
            EnsureFolder(MaterialsFolder);

            Material matFloor = CreateOrLoadMaterial("MAT_GreyFloor", new Color(0.55f, 0.58f, 0.62f));
            Material matSlope = CreateOrLoadMaterial("MAT_GreySlope", new Color(0.45f, 0.52f, 0.58f));
            Material matAccent = CreateOrLoadMaterial("MAT_GreyAccent", new Color(0.75f, 0.65f, 0.32f));
            Material matPortal = CreateOrLoadMaterial("MAT_PortalPad", new Color(0.22f, 0.85f, 0.45f));
            Material matPlayer = CreateOrLoadMaterial("MAT_PlayerBody", new Color(0.25f, 0.68f, 0.92f));
            Material matVisor = CreateOrLoadMaterial("MAT_PlayerVisor", new Color(0.98f, 0.78f, 0.22f));
            Material matRelicFragile = CreateOrLoadMaterial("MAT_RelicCrystal", new Color(0.32f, 0.88f, 0.96f));
            Material matRelicHeavy = CreateOrLoadMaterial("MAT_RelicStone", new Color(0.72f, 0.58f, 0.42f));
            Material matChest = CreateOrLoadMaterial("MAT_GreyChest", new Color(0.58f, 0.40f, 0.22f));

            string hubPath = $"{ScenesFolder}/Temp_Hub.unity";
            string mapPath = $"{ScenesFolder}/Greybox_Prototype.unity";

            BuildTempHubScene(hubPath, matFloor, matPortal, matPlayer, matVisor);
            BuildGreyboxScene(mapPath, matFloor, matSlope, matAccent, matPortal, matPlayer, matVisor, matRelicFragile, matRelicHeavy, matChest);

            EditorBuildSettings.scenes = new[]
            {
                new EditorBuildSettingsScene(hubPath, true),
                new EditorBuildSettingsScene(mapPath, true)
            };

            EditorSceneManager.OpenScene(mapPath, OpenSceneMode.Single);
            Debug.Log("[RelicHaulers] Gate 1 Slice 1A+1B scenes built! Test Carry/Throw/Cancel & GreyboxChest in Greybox_Prototype.");
        }

        private static void BuildTempHubScene(string scenePath, Material matFloor, Material matPortal, Material matPlayer, Material matVisor)
        {
            EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

            CreateBox("Hub_Floor", new Vector3(0f, -0.5f, 0f), new Vector3(18f, 1f, 18f), Vector3.zero, matFloor);
            CreatePortal("DropHatch_ToGreybox", new Vector3(0f, 0.1f, 6f), "Greybox_Prototype", "Press [E] to Drop Into Greybox Map", matPortal);

            GameObject player = CreatePlayer(new Vector3(0f, 0.1f, -3f), matPlayer, matVisor);
            SetupCameraAndHud(player, "GATE 1: TEMP HUB (Stand on Green Pad + [E])");

            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), scenePath);
        }

        private static void BuildGreyboxScene(
            string scenePath,
            Material matFloor,
            Material matSlope,
            Material matAccent,
            Material matPortal,
            Material matPlayer,
            Material matVisor,
            Material matRelicFragile,
            Material matRelicHeavy,
            Material matChest)
        {
            EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

            // Main island
            CreateBox("Main_Floor", new Vector3(0f, -0.5f, 0f), new Vector3(24f, 1f, 24f), Vector3.zero, matFloor);

            // Ramps & Terraces
            CreateBox("Ramp_15Deg", new Vector3(-6f, 1.0f, 6f), new Vector3(4f, 0.5f, 9f), new Vector3(-15f, 0f, 0f), matSlope);
            CreateBox("Terrace_Low", new Vector3(-6f, 2.0f, 13.5f), new Vector3(7f, 1f, 7f), Vector3.zero, matAccent);

            CreateBox("Ramp_26Deg", new Vector3(6f, 1.8f, 6f), new Vector3(4f, 0.5f, 9f), new Vector3(-26f, 0f, 0f), matSlope);
            CreateBox("Terrace_High", new Vector3(6f, 3.6f, 13.5f), new Vector3(7f, 1f, 7f), Vector3.zero, matAccent);

            // Stairs test
            for (int i = 0; i < 8; i++)
            {
                CreateBox($"Step_{i + 1}", new Vector3(0f, 0.125f + i * 0.25f, 4f + i * 0.7f), new Vector3(3.5f, 0.25f, 0.8f), Vector3.zero, matSlope);
            }
            CreateBox("Stairs_TopLanding", new Vector3(0f, 1.9f, 13.5f), new Vector3(5f, 1f, 7f), Vector3.zero, matFloor);

            // Jump gap platforms
            CreateBox("Gap_Island_1", new Vector3(0f, 2.1f, 20f), new Vector3(4f, 0.8f, 4f), Vector3.zero, matAccent);
            CreateBox("Gap_Island_2", new Vector3(0f, 2.5f, 27.5f), new Vector3(4.5f, 0.8f, 4.5f), Vector3.zero, matAccent);
            CreateBox("Goal_Island", new Vector3(0f, 2.8f, 36f), new Vector3(9f, 1f, 9f), Vector3.zero, matFloor);

            // Slice 1B test objects right near spawn
            CreateRelic("Relic_CrystalVase_Sample", new Vector3(-2.5f, 0.5f, -2.5f), new Vector3(0.45f, 0.7f, 0.45f), matRelicFragile, "Bình Pha Lê Mẫu", 180, 2f, 100f, 3.5f);
            CreateRelic("Relic_StoneIdol_Sample", new Vector3(2.5f, 0.5f, -2.5f), new Vector3(0.7f, 0.75f, 0.7f), matRelicHeavy, "Tượng Đá Mẫu (Nặng)", 140, 14f, 320f, 9.0f);
            CreateGreyboxChest(new Vector3(-5.0f, 0f, -2.5f), matChest, matRelicFragile);

            // Stack of loose crates to throw/bash into
            for (int row = 0; row < 2; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    var crate = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    crate.name = $"TargetCrate_{row}_{col}";
                    crate.transform.position = new Vector3(4.2f + col * 0.85f, 0.4f + row * 0.8f, -1.0f);
                    crate.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                    crate.GetComponent<Renderer>().sharedMaterial = matAccent;
                    crate.AddComponent<Rigidbody>().mass = 2.5f;
                }
            }

            CreatePortal("Extract_ReturnToHub", new Vector3(0f, 3.4f, 36f), "Temp_Hub", "Press [E] to Return to Temp Hub", matPortal);

            GameObject player = CreatePlayer(new Vector3(0f, 0.1f, -6f), matPlayer, matVisor);
            SetupCameraAndHud(player, "GATE 1: GREYBOX PROTOTYPE (Slice 1A+1B - Carry/Throw/Chest)");

            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), scenePath);
        }

        private static GameObject CreatePlayer(Vector3 pos, Material matBody, Material matVisor)
        {
            var root = new GameObject("Player_Hauler");
            root.transform.position = pos;

            var capsule = root.AddComponent<CapsuleCollider>();
            capsule.center = new Vector3(0f, 1.02f, 0f);
            capsule.height = 1.35f;
            capsule.radius = 0.35f;

            root.AddComponent<Rigidbody>();
            root.AddComponent<HaulerInputReader>();
            var controller = root.AddComponent<FloatingCapsuleController>();
            controller.SetSpawnPosition(pos);
            root.AddComponent<CarrySystem>();

            var visualRoot = new GameObject("VisualRoot");
            visualRoot.transform.SetParent(root.transform, false);
            visualRoot.AddComponent<SquashStretchVisual>();

            var body = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            body.name = "BodyMesh";
            Object.DestroyImmediate(body.GetComponent<Collider>());
            body.transform.SetParent(visualRoot.transform, false);
            body.transform.localPosition = new Vector3(0f, 0.8f, 0f);
            body.transform.localScale = new Vector3(0.7f, 0.8f, 0.7f);
            body.GetComponent<Renderer>().sharedMaterial = matBody;

            var visor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            visor.name = "Visor";
            Object.DestroyImmediate(visor.GetComponent<Collider>());
            visor.transform.SetParent(visualRoot.transform, false);
            visor.transform.localPosition = new Vector3(0f, 1.25f, 0.30f);
            visor.transform.localScale = new Vector3(0.45f, 0.22f, 0.22f);
            visor.GetComponent<Renderer>().sharedMaterial = matVisor;

            var pack = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pack.name = "Backpack";
            Object.DestroyImmediate(pack.GetComponent<Collider>());
            pack.transform.SetParent(visualRoot.transform, false);
            pack.transform.localPosition = new Vector3(0f, 0.9f, -0.36f);
            pack.transform.localScale = new Vector3(0.48f, 0.58f, 0.28f);
            pack.GetComponent<Renderer>().sharedMaterial = matVisor;

            return root;
        }

        private static void CreateRelic(string name, Vector3 pos, Vector3 scale, Material mat, string displayName, int value, float massKg, float durability, float breakSpeed)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = name;
            go.transform.position = pos;
            go.transform.localScale = scale;
            go.GetComponent<Renderer>().sharedMaterial = mat;

            var relic = go.AddComponent<RelicItem>();
            SerializedObject so = new SerializedObject(relic);
            so.FindProperty("displayName").stringValue = displayName;
            so.FindProperty("baseValue").intValue = value;
            so.FindProperty("massKg").floatValue = massKg;
            so.FindProperty("maxDurability").floatValue = durability;
            so.FindProperty("breakThresholdSpeed").floatValue = breakSpeed;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void CreateGreyboxChest(Vector3 groundPos, Material chestMat, Material lootMat)
        {
            var chestGo = new GameObject("Chest_Greybox");
            chestGo.transform.position = groundPos;

            var body = GameObject.CreatePrimitive(PrimitiveType.Cube);
            body.name = "ChestBody";
            body.transform.SetParent(chestGo.transform, false);
            body.transform.localPosition = new Vector3(0f, 0.32f, 0f);
            body.transform.localScale = new Vector3(1.2f, 0.64f, 0.85f);
            body.GetComponent<Renderer>().sharedMaterial = chestMat;

            var lidPivot = new GameObject("LidPivot");
            lidPivot.transform.SetParent(chestGo.transform, false);
            lidPivot.transform.localPosition = new Vector3(0f, 0.64f, -0.425f);

            var lid = GameObject.CreatePrimitive(PrimitiveType.Cube);
            lid.name = "ChestLid";
            lid.transform.SetParent(lidPivot.transform, false);
            lid.transform.localPosition = new Vector3(0f, 0.12f, 0.425f);
            lid.transform.localScale = new Vector3(1.22f, 0.24f, 0.86f);
            lid.GetComponent<Renderer>().sharedMaterial = chestMat;

            var spawnPoint = new GameObject("LootSpawn");
            spawnPoint.transform.SetParent(chestGo.transform, false);
            spawnPoint.transform.localPosition = new Vector3(0f, 0.95f, 0.1f);

            var trigger = chestGo.AddComponent<SphereCollider>();
            trigger.isTrigger = true;
            trigger.radius = 1.6f;

            var chest = chestGo.AddComponent<GreyboxChest>();
            SerializedObject so = new SerializedObject(chest);
            so.FindProperty("lidPivot").objectReferenceValue = lidPivot.transform;
            so.FindProperty("spawnPoint").objectReferenceValue = spawnPoint.transform;
            so.FindProperty("lootMaterial").objectReferenceValue = lootMat;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void SetupCameraAndHud(GameObject player, string label)
        {
            Camera cam = Camera.main;
            if (cam == null)
            {
                var camGo = new GameObject("Main Camera");
                cam = camGo.AddComponent<Camera>();
                camGo.tag = "MainCamera";
            }

            var camRig = cam.gameObject.GetComponent<GreyboxCameraRig>() ?? cam.gameObject.AddComponent<GreyboxCameraRig>();
            camRig.SetTarget(player.transform, player.GetComponent<HaulerInputReader>());

            var hudGo = new GameObject("Greybox_HUD");
            var hud = hudGo.AddComponent<GreyboxHudOverlay>();
            SerializedObject hudSo = new SerializedObject(hud);
            hudSo.FindProperty("player").objectReferenceValue = player.GetComponent<FloatingCapsuleController>();
            hudSo.FindProperty("carrySystem").objectReferenceValue = player.GetComponent<CarrySystem>();
            hudSo.FindProperty("stageLabel").stringValue = label;
            hudSo.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void CreateBox(string name, Vector3 pos, Vector3 scale, Vector3 euler, Material mat)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = name;
            go.transform.position = pos;
            go.transform.rotation = Quaternion.Euler(euler);
            go.transform.localScale = scale;
            go.GetComponent<Renderer>().sharedMaterial = mat;
        }

        private static void CreatePortal(string name, Vector3 pos, string targetScene, string prompt, Material mat)
        {
            var pad = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            pad.name = name;
            pad.transform.position = pos;
            pad.transform.localScale = new Vector3(3.2f, 0.15f, 3.2f);
            pad.GetComponent<Renderer>().sharedMaterial = mat;

            var col = pad.GetComponent<Collider>();
            col.isTrigger = true;

            var portal = pad.AddComponent<ScenePortalPad>();
            SerializedObject so = new SerializedObject(portal);
            so.FindProperty("targetSceneName").stringValue = targetScene;
            so.FindProperty("promptText").stringValue = prompt;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        private static Material CreateOrLoadMaterial(string name, Color color)
        {
            string path = $"{MaterialsFolder}/{name}.mat";
            var existing = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (existing != null)
            {
                return existing;
            }

            Shader shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
            var mat = new Material(shader) { color = color };
            if (mat.HasProperty("_BaseColor"))
            {
                mat.SetColor("_BaseColor", color);
            }
            AssetDatabase.CreateAsset(mat, path);
            return mat;
        }

        private static void EnsureFolder(string assetPath)
        {
            if (AssetDatabase.IsValidFolder(assetPath))
            {
                return;
            }

            int idx = assetPath.LastIndexOf('/');
            string parent = assetPath.Substring(0, idx);
            string child = assetPath.Substring(idx + 1);
            AssetDatabase.CreateFolder(parent, child);
        }
    }
}
#endif
