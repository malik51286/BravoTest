using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.Tilemaps;

    public class BulletTest
    {
        private GameObject _bulletPlayer;
        private GameObject _bulletEnemy;
        private GameObject _playerTank;
        private GameObject _enemyTank;
        private GameObject _trophy;
        private GameObject _grid;
        private GameObject _tilemap1;
        private GameObject _tilemap2;
        private GameObject _tilemap3;

        [SetUp]
        public void Setup()
        {
        }

        [TearDown]
        public void Teardown()
        {
        }
  
        [UnityTest]
        public IEnumerator PlayerBulletHitEnemy()
        {
            _bulletPlayer = MonoBehaviour.Instantiate(Resources.Load<GameObject>(ResourceList.CommonPath + ResourceList.PlayerBullet));
            _enemyTank = MonoBehaviour.Instantiate(Resources.Load<GameObject>(ResourceList.CommonPath + ResourceList.EnemyTank));
            _bulletPlayer.transform.position = Vector3.zero;
            _enemyTank.transform.position = Vector3.zero;
            yield return null;
            Assert.Less(_enemyTank.GetComponent<IDestroyable>().health, 0.1f);
            yield return new WaitForSeconds(0.1f);
            UnityEngine.Assertions.Assert.IsNull(_bulletPlayer);
            yield return new WaitForSeconds(2f);
            UnityEngine.Assertions.Assert.IsNull(_enemyTank);
        }

        [UnityTest]
        public IEnumerator EnemyBulletHitPlayer()
        {
            _bulletEnemy = MonoBehaviour.Instantiate(Resources.Load<GameObject>(ResourceList.CommonPath + ResourceList.EnemyBullet));
            _playerTank = MonoBehaviour.Instantiate(Resources.Load<GameObject>(ResourceList.CommonPath + ResourceList.PlayerTank));
            _bulletEnemy.transform.position = Vector3.zero;
            _playerTank.transform.position = Vector3.zero;
            yield return null;
            Assert.Less(_playerTank.GetComponent<IDestroyable>().health, 0.1f);
            yield return new WaitForSeconds(0.1f);
            UnityEngine.Assertions.Assert.IsNull(_bulletEnemy);
            yield return new WaitForSeconds(2f);
            UnityEngine.Assertions.Assert.IsNull(_playerTank);
        }

        [UnityTest]
        public IEnumerator PlayerBulletHitEnemyBullet()
        {
            _bulletPlayer = MonoBehaviour.Instantiate(Resources.Load<GameObject>(ResourceList.CommonPath + ResourceList.PlayerBullet));
            _bulletEnemy = MonoBehaviour.Instantiate(Resources.Load<GameObject>(ResourceList.CommonPath + ResourceList.EnemyBullet));
            _bulletPlayer.transform.position = Vector3.zero;
            _bulletEnemy.transform.position = Vector3.zero;
            yield return new WaitForSeconds(0.1f);
            UnityEngine.Assertions.Assert.IsNull(_bulletPlayer);
            UnityEngine.Assertions.Assert.IsNull(_bulletEnemy);
        }

        [UnityTest]
        public IEnumerator EnemyBulletHitTrophy()
        {
            _bulletEnemy = MonoBehaviour.Instantiate(Resources.Load<GameObject>(ResourceList.CommonPath + ResourceList.EnemyBullet));
            _trophy = MonoBehaviour.Instantiate(Resources.Load<GameObject>(ResourceList.CommonPath + ResourceList.Trophy));
            _bulletEnemy.transform.position = Vector3.zero;
            _trophy.transform.position = Vector3.zero;
            yield return new WaitForFixedUpdate();
            Assert.Less(_trophy.GetComponent<IDestroyable>().health, 0.1f);
            yield return new WaitForSeconds(0.1f);
            UnityEngine.Assertions.Assert.IsNull(_bulletEnemy);
            UnityEngine.Assertions.Assert.IsNull(_trophy);
        }

        [UnityTest]
        public IEnumerator PlayerBulletHitTrophy()
        {
            _bulletPlayer = MonoBehaviour.Instantiate(Resources.Load<GameObject>(ResourceList.CommonPath + ResourceList.PlayerBullet));
            _trophy = MonoBehaviour.Instantiate(Resources.Load<GameObject>(ResourceList.CommonPath + ResourceList.Trophy));
            _bulletPlayer.transform.position = Vector3.zero;
            _trophy.transform.position = Vector3.zero;
            yield return new WaitForFixedUpdate();
            Assert.AreEqual(_trophy.GetComponent<IDestroyable>().health,1f);
            yield return new WaitForSeconds(0.1f);
            UnityEngine.Assertions.Assert.IsNotNull(_bulletPlayer);
            UnityEngine.Assertions.Assert.IsNotNull(_trophy);
        }

        [UnityTest]
        public IEnumerator PlayerBulletHitTileMap1()
        {
            _grid = MonoBehaviour.Instantiate(Resources.Load<GameObject>(ResourceList.CommonPath + ResourceList.Grid));
            _tilemap1 = MonoBehaviour.Instantiate(Resources.Load<GameObject>(ResourceList.CommonPath + ResourceList.TileMap+0));

            _grid.transform.position = Vector3.zero;
            _tilemap1.transform.parent = _grid.transform;

            Tilemap tilemap = _tilemap1.GetComponent<Tilemap>();
            BoundsInt bounds = tilemap.cellBounds;
            TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
            yield return new WaitForSeconds(0.1f);

            foreach (var position in tilemap.cellBounds.allPositionsWithin)
            {
                if (tilemap.HasTile(position))
                {
                    if (tilemap.GetTile(position).name == "tilemap-level_0")
                    {
                        _bulletPlayer = MonoBehaviour.Instantiate(Resources.Load<GameObject>(ResourceList.CommonPath + ResourceList.PlayerBullet));
                        _bulletPlayer.transform.position = tilemap.CellToWorld(position);
                        yield return new WaitForSeconds(0.1f);
                        UnityEngine.Assertions.Assert.IsNull(_bulletPlayer);
                        UnityEngine.Assertions.Assert.IsNull(tilemap.GetTile(position));
                    }
                }
            }
            Object.Destroy(_tilemap1);
            Object.Destroy(_grid);
    }

        [UnityTest]
        public IEnumerator EnemyBulletHitTileMap1()
        {
            _grid = MonoBehaviour.Instantiate(Resources.Load<GameObject>(ResourceList.CommonPath + ResourceList.Grid));
            _tilemap1 = MonoBehaviour.Instantiate(Resources.Load<GameObject>(ResourceList.CommonPath + ResourceList.TileMap + 0));

            _grid.transform.position = Vector3.zero;
            _tilemap1.transform.parent = _grid.transform;

            Tilemap tilemap = _tilemap1.GetComponent<Tilemap>();
            BoundsInt bounds = tilemap.cellBounds;
            TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
            yield return new WaitForSeconds(0.1f);

            foreach (var position in tilemap.cellBounds.allPositionsWithin)
            {
                if (tilemap.HasTile(position))
                {
                    if (tilemap.GetTile(position).name == "tilemap-level_0")
                    {
                        _bulletEnemy = MonoBehaviour.Instantiate(Resources.Load<GameObject>(ResourceList.CommonPath + ResourceList.EnemyBullet));
                        _bulletEnemy.transform.position = tilemap.CellToWorld(position);
                        yield return new WaitForSeconds(0.1f);
                        UnityEngine.Assertions.Assert.IsNull(_bulletEnemy);
                        UnityEngine.Assertions.Assert.IsNull(tilemap.GetTile(position));
                    }
                }
            }
            Object.Destroy(_tilemap1);
            Object.Destroy(_grid);
    }


}
