export default interface UpdateClassModel {
  name: string;
  isAbstract: boolean;
  isInterface: boolean;
  isSealed: boolean;
  isVirtual: boolean;
  parent: string | null;
}