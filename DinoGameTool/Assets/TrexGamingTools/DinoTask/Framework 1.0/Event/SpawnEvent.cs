using System;
using UnityEngine;

namespace Dino_Core.Task
{
    public abstract class SpawnEvent : BaseEvent
    {
        public float Interval;
        public SpawnItem[] Items;

        public int CurrentItem
        {
            get; set;
        }

        protected float _nextTime;
        protected int _itemCount;
        protected Transform _handleTransform;

        /// <summary>
        /// 创建物体的方法
        /// </summary>
        /// <param name="_poolName"></param>
        /// <param name="_curRes"></param>
        /// <param name="_postion"></param>
        /// <returns></returns>
        protected virtual Transform SpawnFunc(string _poolName, TaskConst.Enemy_Type _type, string _args, Vector3 _postion, Transform _movePoint)
        {
            return null;
        }
        protected override void TEnd()
        {
            CurrentItem = 0;
            _itemCount = Items[0].Number;
        }
        protected override void TStart()
        {
            CurrentItem = 0;
            _itemCount = Items[0].Number;

            // 第一次开始
            _nextTime = 0;
        }
        protected override void TReset()
        {
            TEnd();
        }
        protected override void OnExcute()
        {
            if (_nextTime < Time.time)
            {
                // 创建实体，这里会调用子类覆盖的方法，具体实现参看子类实现
                _handleTransform = SpawnFunc("AI",
                    Items[CurrentItem].Type,
                    Items[CurrentItem].Args,
                    transform.position,
                    Items[CurrentItem].MovePosition);

                // 设置回调和AI类型，回调用于AI死的时候告诉任务系统方便做统计
                //_handleDamager = _handleTransform.GetComponent<DamageHandler>();
                //_handleDamager.AIType = Items[CurrentItem].Type;
                //_handleDamager.EntityDeadEventHandler = UnregisterEntity;

                // 将这个物体添加到任务系统中，方便统计
                RegisterEntity(Items[CurrentItem].Type, _handleTransform);

                _itemCount--;
                if (_itemCount <= 0)
                {
                    CurrentItem++;
                    if (CurrentItem == Items.Length)
                    {
                        EndEvent();
                    }

                    // 下一波
                    _nextTime = Time.time + Interval;
                    _itemCount = Items[CurrentItem].Number;

                    return;
                }
                _nextTime = Time.time + Items[CurrentItem].Interval;
            }
        }
    }
}