using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Dino_Core.DinoNav2D {

    [RequireComponent(typeof(Rigidbody2D))]
    public class Nav2DAgent : MonoBehaviour {

        private Rigidbody2D m_Rigidbody = null;

        [SerializeField]
        private float Speed = 2;

        [SerializeField]
        private float Acc = 0.4f;

        [SerializeField]
        private float MaxSnap = 0.80f;

        [SerializeField]
        private float Dampin = 0.90f;

        private float m_rotationDelta = 0.0f;

        [SerializeField]
        private readonly float mRotationLerp = 0.1f;

        [SerializeField]
        private List<Nav2DNode> mPath;

        private int mCurrentStep = 0;

        private Vector3 mCurrentTarget = new Vector3();

        private Vector3 mCurrentFacePoint = new Vector3();

        private bool mIsEnd = false;

        public void NavTo(Vector3 _pos)
        {
            mPath = Nav2DProccesser.Calculate_Navigation(transform.position,_pos);
            mCurrentStep = 0;
            NextPoint();

            StartCoroutine("_excuteMovingAgent");
        }
        public void MoveTo(Vector3 _pos)
        {
            mCurrentTarget = _pos;
            mCurrentFacePoint = _pos;

            mPath.Clear();
            mCurrentStep = 0;

            StartCoroutine("_excuteMovingAgent");
        }
        public void FaceTo(Vector3 _pos)
        {
            mCurrentFacePoint = _pos;
        }

        public void SetSpeed(float _speed)
        {
            this.Speed = _speed;
        }
        public void SetSpeed(float _speed, float _acc)
        {
            this.Speed = _speed;
            this.Acc = _acc;
        }
        public void SetAcc(float _acc)
        {
            this.Acc = _acc;
        }

        private void NextPoint()
        {
            if (mPath == null || mPath.Count == 0)
            {
                mCurrentTarget = transform.position;
                mIsEnd = true;
            }
            else
            {
                if(mCurrentStep < mPath.Count && mPath[mCurrentStep] != null)
                {
                    // 下一个目标
                    mCurrentTarget = mPath[mCurrentStep++].NodePosition;
                    mCurrentFacePoint = mCurrentTarget;
                }
                else
                {
                    // 已经到达目的地
                    mCurrentTarget = transform.position;
                    mIsEnd = true;
                }
            }
        }

        private bool isCloseEnough(Vector3 _posA, Vector3 _posB)
        {
            return Vector3.Distance(_posA,_posB) < MaxSnap;
        }

        private void ClampSpeed()
        {
            if (m_Rigidbody.velocity.magnitude > 0.05f)
            {
                m_Rigidbody.velocity *= Dampin;
            }
            else
            {
                m_Rigidbody.velocity = new Vector2(0, 0);
            }
        }

        void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            ClampSpeed();
            LerpFaceTo(mCurrentFacePoint);
        }

        private IEnumerator _excuteMovingAgent()
        {
            mIsEnd = false;
            while (!mIsEnd)
            {
                Vector3 _dir = mCurrentTarget - transform.position;

                if (m_Rigidbody.velocity.magnitude < Speed)
                {
                    m_Rigidbody.velocity += new Vector2(_dir.normalized.x, _dir.normalized.y) * Acc;
                }

                // 是不是离目标点足够进
                if (isCloseEnough(mCurrentTarget, transform.position))
                {
                    // 下一个目标点
                    NextPoint();
                }

                yield return null;
            }
        }

        private void LerpFaceTo(Vector3 _pos)
        {
            if(Vector3.Distance(_pos, transform.position) < 0.05f)
            {
                return;
            }

            Vector3 transPoint = new Vector3(transform.position.x, transform.position.y, 0);
            Vector3 dir = (_pos - transPoint).normalized;

            if (Vector3.Cross(Vector3.up, dir).z > 0)
                m_rotationDelta = Vector3.Angle(Vector3.up, dir);
            else
                m_rotationDelta = -Vector3.Angle(Vector3.up, dir);

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(m_rotationDelta, Vector3.forward), mRotationLerp);
        }
    }
}