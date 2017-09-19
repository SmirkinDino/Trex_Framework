using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EDefaultController
{

}

public class Creature : MonoBehaviour {
    
    public enum ECreatureController
    {
        
    }

    /// <summary>
    /// 这个之后会放到main里面，暂停用
    /// </summary>
    public static bool m_IsGameRunning;

    private static Dictionary<EDefaultController, IActionController>.Enumerator m_HandleEnumerator;

    private static IActionController m_HandleController;

    /// <summary>
    /// current Controller
    /// </summary>
    protected IActionController m_CurrentAuthorityController;

    /// <summary>
    /// Controller set
    /// </summary>
    protected Dictionary<ECreatureController, IActionController> m_Controllers;

    /// <summary>
    /// DefaultController set
    /// </summary>
    protected Dictionary<EDefaultController, IActionController> m_DefaultControllers;

    public virtual void Init()
    {
        if (m_Controllers == null)
        {
            m_Controllers = new Dictionary<ECreatureController, IActionController>();
        }
        else
        {
            m_Controllers.Clear();
        }

        if (m_DefaultControllers == null)
        {
            m_DefaultControllers = new Dictionary<EDefaultController, IActionController>();
        }
        else
        {
            m_DefaultControllers.Clear();
        }

        //m_Controllers.Add();
        //m_Controllers.Add();
        //m_Controllers.Add();
        //m_Controllers.Add();
        //m_Controllers.Add();
        //m_Controllers.Add();
        //m_Controllers.Add();
        //m_Controllers.Add();
    }

    public virtual void OnUpdate()
    {

    }

    public virtual void OnFixedUpdate()
    {

    }

    public virtual void OnLatedUpdate()
    {

    }



    public void AuthorController(ECreatureController _type)
    {
        m_HandleController = null;

        m_Controllers.TryGetValue(_type,out m_HandleController);

        if (m_HandleController != null)
        {
            m_HandleController.OnTakeAuthority();

            if (m_CurrentAuthorityController != null)
            {
                m_CurrentAuthorityController.OnLoseAuthority();

                m_CurrentAuthorityController = m_HandleController;
            }
        }
    }

    public void EnableDefaultController(EDefaultController _type, bool _enable)
    {
        m_HandleController = null;

        m_DefaultControllers.TryGetValue(_type, out m_HandleController);

        if (m_HandleController != null)
        {
            if (_enable && !m_HandleController.Enable)
            {
                m_HandleController.OnTakeAuthority();
                m_HandleController.Enable = _enable;
                return;
            }

            if (!_enable && m_HandleController.Enable)
            {
                m_HandleController.OnLoseAuthority();
                m_HandleController.Enable = _enable;
                return;
            }
        }
    }

    private void Update()
    {
        if (!m_IsGameRunning)
        {
            return;
        }

        OnUpdate();

        m_HandleEnumerator = m_DefaultControllers.GetEnumerator();

        while (m_HandleEnumerator.MoveNext())
        {
            if (m_HandleEnumerator.Current.Value != null)
            {
                m_HandleEnumerator.Current.Value.UpdateAction();
            }
        }

        if (m_CurrentAuthorityController != null)
        {
            m_CurrentAuthorityController.UpdateAction();
        }
    }

    private void FixedUpdate()
    {
        if (!m_IsGameRunning)
        {
            return;
        }

        OnFixedUpdate();

        m_HandleEnumerator = m_DefaultControllers.GetEnumerator();

        while (m_HandleEnumerator.MoveNext())
        {
            if (m_HandleEnumerator.Current.Value != null)
            {
                m_HandleEnumerator.Current.Value.FixedUpdateAction();
            }
        }

        if (m_CurrentAuthorityController != null)
        {
            m_CurrentAuthorityController.FixedUpdateAction();
        }
    }

    private void LateUpdate()
    {
        if (!m_IsGameRunning)
        {
            return;
        }

        OnLatedUpdate();

        m_HandleEnumerator = m_DefaultControllers.GetEnumerator();

        while (m_HandleEnumerator.MoveNext())
        {
            if (m_HandleEnumerator.Current.Value != null)
            {
                m_HandleEnumerator.Current.Value.LatedUpdateAction();
            }
        }

        if (m_CurrentAuthorityController != null)
        {
            m_CurrentAuthorityController.LatedUpdateAction();
        }
    }

    private void OnDestroy()
    {
        m_HandleController = null;
        m_HandleEnumerator.Dispose();
    }

}
