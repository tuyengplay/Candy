﻿// // ©2015 - 2024 Candy Smith
// // All rights reserved
// // Redistribution of this software is strictly not allowed.
// // Copy of this software can be obtained from unity asset store only.
// // THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// // IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// // FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
// // AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// // LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// // OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// // THE SOFTWARE.

using System;
using UnityEngine;

namespace SweetSugar.Scripts.MapScripts
{
    public class ApiTest : MonoBehaviour, IMapProgressManager
    {
        private int _levelNumber = 1;
        private int _starsCount = 1;
        private bool _isShow;

        public DemoButton YesButton;
        public DemoButton NoButton;
        public GameObject ConfirmationView;
        public int SelectedLevelNumber;

        public void Awake()
        {
            //Uncomment to set this script as IMapProgressManager
            //LevelsMap.OverrideMapProgressManager(this);
        }

        #region Events

        public void OnEnable()
        {
            Debug.Log("Subscribe to events.");
        }

        public void OnDisable()
        {
            Debug.Log("Unsubscribe from events.");
        }

        private void OnLevelReached(object sender, LevelReachedEventArgs e)
        {
            Debug.Log(string.Format("Level {0} reached.", e.Number));
        }

        #endregion

        #region Api test
        public void OnGUI()
        {
            GUILayout.BeginVertical();

            DrawToggleShowButton();

            if (_isShow)
            {
                DrawInputParameters();
                if (GUILayout.Button("Complete all  levels"))
                {
                    for (var i = 1; i < GameObject.Find("Levels").transform.childCount; i++)
                    {
                        // LevelsMap.CompleteLevel(i, _starsCount);
                        SaveLevelStarsCount(i, _starsCount);
                    }
                }
            }

            GUILayout.EndVertical();
        }

        private void DrawToggleShowButton()
        {
            if (!_isShow)
            {
                if (GUILayout.Button("Show API tests"))
                {
                    _isShow = true;
                }
            }
            if (_isShow)
            {
                if (GUILayout.Button("Hide API tests"))
                {
                    _isShow = false;
                }
            }
        }

        private void DrawInputParameters()
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label("Level number:");
            var strLevelNumber = GUILayout.TextField(_levelNumber.ToString(), 10, GUILayout.Width(80));
            int.TryParse(strLevelNumber, out _levelNumber);

          

            GUILayout.EndHorizontal();
        }

        #endregion

        #region IMapProgressManager
        private string GetLevelKey(int number)
        {
            return string.Format("Level.{0:000}.StarsCount", number);
        }
        
        public string GetScoreKey(int number)
        {
            throw new NotImplementedException();
        }

        public void SaveLevelStarsCount(int level, int starsCount, int score)
        {
            throw new NotImplementedException();
        }
        
        public int LoadLevelStarsCount(int level)
        {
            return level > 10 ? 0 : (level % 3 + 1);
        }

        public void SaveLevelStarsCount(int level, int starsCount)
        {
            Debug.Log(string.Format("Stars count {0} of level {1} saved.", starsCount, level));
            PlayerPrefs.SetInt(GetLevelKey(level), starsCount);

        }

        public void ClearLevelProgress(int level)
        {

        }

        #endregion

        #region Confirmation demo

        

        private void OnNoButtonClick(object sender, EventArgs e)
        {
            ConfirmationView.SetActive(false);
        }

      

        public int GetLastLevel()
        {
            return 0;
        }

        string IMapProgressManager.GetLevelKey(int number)
        {
            return GetLevelKey(number);
        }
        
        #endregion

    }
}
