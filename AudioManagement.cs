using HutongGames.PlayMaker.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SwordQi
{
    public class AudioManagement
    {
        public static void SetAudioClip(GameObject obj , AudioClip audio)
        {
            //播放音效案例
            obj.AddComponent<AudioSource>();//给对象添加音频组件，用来播放音频
            AudioSource source = obj.GetComponent<AudioSource>();
            source.clip = audio;//指定音频片段是那个文件，文件类型为：AudioClip
            source.volume = 16;//音量，不知道默认值是多少
            source.rolloffMode = AudioRolloffMode.Logarithmic;//衰减模式,对数的,Linear;线性的,Custom;常规
            source.maxDistance = 100;//最大距离,离开这个范围应该就听不到了
            //source.Play();//播放
        }

    }
}
