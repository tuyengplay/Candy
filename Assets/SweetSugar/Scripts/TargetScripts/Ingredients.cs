// // ©2015 - 2024 Candy Smith
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

using System.Linq;
using SweetSugar.Scripts.Core;
using SweetSugar.Scripts.Level;
using SweetSugar.Scripts.System;
using SweetSugar.Scripts.TargetScripts.TargetSystem;
using UnityEngine;

namespace SweetSugar.Scripts.TargetScripts
{
    /// <summary>
    /// Ingredients target
    /// </summary>
    public class Ingredients : Target
    {
        public int[] fulledCountPerLevel;
        public override int CountTarget()
        {
            return amount;
        }

        public override int CountTargetSublevel()
        {
            return amount;
        }

        public override void InitTarget(LevelData levelData)
        {
            foreach (var item in subTargetContainers)
            {
                amount += item.GetCount();
            }
            destAmount = amount;
            if (Application.isPlaying)
                fulledCountPerLevel = new int[LevelManager.THIS.fieldBoards.Count];

        }

        public override void DestroyEvent(GameObject obj)
        {


        }

        public override void FulfillTarget<T>(T[] items)
        {
        }

        public override void CheckTargetItemsAfterDestroy()
        {
            if (fulledCountPerLevel.Length == 0)
                fulledCountPerLevel = new int[LevelManager.THIS.fieldBoards.Count];
            var sqList = LevelManager.THIS.field.GetBottomRow();
            foreach (var item in subTargetContainers)
            {
                foreach (var hItem in sqList)
                {
                    if (hItem.Item == null) continue;
                    var obj = hItem.Item;
                    var ingredientName = obj.sprRenderer.FirstOrDefault().sprite.name;
                    if (item.extraObject.name == ingredientName && item.preCount > 0)
                    {
                        var v = LevelManager.THIS.fieldBoards.FindIndex(x => x == LevelManager.THIS.field);
                        fulledCountPerLevel[v]++;
                        amount--;
                        item.preCount--;
                        var pos = TargetGUI.GetTargetGUIPosition(ingredientName);
                        var itemAnim = new GameObject();
                        var animComp = itemAnim.AddComponent<AnimateItems>();
                        LevelManager.THIS.animateItems.Add(animComp);
                        animComp.InitAnimation(obj.gameObject, pos, obj.transform.localScale, () => { item.changeCount(-1);
//                        obj.DestroyBehaviour();
                        });
                        obj.DestroyBehaviour();

                    }
                    else if (item.extraObject.name == ingredientName && item.preCount <= 0)
                        obj.DestroyBehaviour();

                }
            }
        }


        public override int GetDestinationCount()
        {
            return destAmount;
        }

        public override int GetDestinationCountSublevel()
        {
            return destAmount;
        }

        public override bool IsTargetReachedSublevel()
        {
            if (fulledCountPerLevel.Length == 0)
                fulledCountPerLevel = new int[LevelManager.THIS.fieldBoards.Count];
            return fulledCountPerLevel[LevelManager.THIS.fieldBoards.FindIndex(x => x == LevelManager.THIS.field)] >= GetDestinationCountSublevel();
        }


        public override bool IsTotalTargetReached()
        {
            return amount <= 0;
        }

        public override int GetCount(string spriteName)
        {
            foreach (var item in subTargetContainers)
            {
                if (item.extraObject.name == spriteName)
                    return item.GetCount();
            }

            return 0;
        }

        // public override bool IsIngredientRequire()
        // {
        //     var items = GameObject.FindObjectsOfType(typeof(ItemIngredient)) as ItemIngredient[];
        //     if (items.Count() == 0) return !IsTotalTargetReached();
        //     var IngredientsOnFieldNotEnough = from obj in items
        //                                       group obj by obj.GetComponent<SpriteRenderer>().sprite.name into g
        //                                       where g.Count() < GetCount(g.Key)
        //                                       select new { SpriteName = g.Key, Count = g.Count() };
        //     Debug.Log(" IngredientsOnFieldNotEnough count " + IngredientsOnFieldNotEnough.Count());
        //     return IngredientsOnFieldNotEnough.Count() > 0;
        // }
    }
}