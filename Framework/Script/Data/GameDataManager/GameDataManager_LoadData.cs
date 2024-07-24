using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Plugins.SimpleZip;
using SimpleJSON;

public partial class GameDataManager
{
    private List<string> _loadDataList = new List<string>();
    /// <summary> CRC(GameData를 압축하여 길이와 저장한 데이터)를 Local과 Cdn을 비교하여 GameData가 바뀌었는지 체크 </summary>
    public async UniTask CheckNewGameData()
    {
        string currentCRC = _GetLocalCRC();
        string newCRC = await WebUtil.WebGet(_GetCdnCRCPath());
        
        //Data Hash 비교? CRC 비교?
        bool isNewData = currentCRC != newCRC;

        string gameData = string.Empty;
        if (isNewData)
        {   
            //Todo;GameData를 새로 다운로드 받을 것인지 체크
            
            
            gameData = await _LoadCdnGameData();
            await _SaveNewData(newCRC, gameData);
        }
        else
        {   //Local에 있는 GameData 사용
            gameData = _GetLocalGameData();
        }

        _LoadData(gameData);
    }

    /// <summary> Cdn에 있는 GameData Load </summary>
    private async UniTask<string> _LoadCdnGameData()
    {
        //Load New GameData    
        string newData = await WebUtil.WebGet(_GetCdnGameDataPath(), (float percent) =>
        {   //Todo;UI에 다운로드 받는 percent 체크
            
        });

        return newData;
    }

    /// <summary> gameData를 풀어서 Dictionary에 저장 </summary>
    private void _LoadData(string gameData)
    {
        var jsonGameData = JSON.Parse(gameData) as JSONObject;
        if (jsonGameData == null)
        {
            DebugManager.LogError($"JSONParsingError: {gameData}", true);
            return;
        }

        foreach (KeyValuePair<string, JSONNode> keyValuePair in jsonGameData)
        {
            var fileName = Zip.Decompress(keyValuePair.Key);
            if (string.IsNullOrEmpty(fileName))
                continue;
            if (_loadDataList.Contains(fileName))
            {
                DebugManager.LogError($"{fileName} is Duplicate");
                continue;
            }
            var data = Zip.Decompress(keyValuePair.Value);
            if (string.IsNullOrEmpty(data))
                continue;
        
            var jsonObject = JSON.Parse(data) as JSONObject;
            if (jsonObject == null)
                continue;

            string key = JsonLoader.GetString(jsonObject, GlobalString.ManagerType);
            if (EnumUtil.TryParse<GlobalEnum.eTemplateManagerType>(key, out var type) == false)
                continue;
            var manager = _CreateOrGetTemplateManager(type);
            if (manager == null)
                continue;

            if (manager.LoadData(fileName, jsonObject) == false)
            {
                DebugManager.LogError($"{type} Manager Load Error");
            }

            _loadDataList.Add(fileName);
            _templateDictionary[type] = manager;
        }
    }

    /// <summary> Cdn에서 받아온 새로운 데이터를 Local에 저장 </summary>
    /// <param name="newCRC"> 저장할 새로운 CRC 데이터 </param> <param name="newGameData"> 저장할 새로운 게임 데이터 </param>
    private async UniTask _SaveNewData(string newCRC, string newGameData)
    {
        var encodeCRC = MD5CryptoUtil.EncodeMD5(newCRC);
        var encodeGameData = MD5CryptoUtil.EncodeMD5(newGameData);
        
        await File.WriteAllTextAsync(_GetLocalCRCPath(), encodeCRC);
        await File.WriteAllTextAsync(_GetLocalGameData(), encodeGameData);
    }

    private string _GetLocalCRC()
    {
        try
        {
            if (File.Exists(_GetLocalCRCPath()))
            {
                using (StreamReader reader = File.OpenText(_GetLocalCRCPath()))
                {
                    var crc = reader.ReadToEnd();
                    var decodeCRC = MD5CryptoUtil.DecodeMD5(crc);
                    
                    return decodeCRC;
                }
            }
        }
        catch (Exception e)
        {
            DebugManager.LogError(e.Message, true);
            return string.Empty;
        }
        
        return string.Empty;
    }

    private string _GetLocalGameData()
    {
        try
        {
            if (File.Exists(_GetLocalGameDataPath()))
            {
                using (StreamReader reader = File.OpenText(_GetLocalGameDataPath()))
                {
                    var gameData = reader.ReadToEnd();
                    var decodeGameData = MD5CryptoUtil.DecodeMD5(gameData);

                    return decodeGameData;
                }
            }
        }
        catch (Exception e)
        {
            DebugManager.LogError(e.Message, true);
            return string.Empty;
        }
        
        return string.Empty;
    }
}
