using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IUsableItem
{
    void UseItem(GameObject target); // �^�[�Q�b�g�ɃA�C�e�����g�p����
    List<string> GetTargetTags();    // �g�p�ΏۂƂȂ�I�u�W�F�N�g�̃^�O�̃��X�g���擾����

}
