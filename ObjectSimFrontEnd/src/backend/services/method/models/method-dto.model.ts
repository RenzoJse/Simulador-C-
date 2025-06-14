export default interface MethodDTO {
    name: string;
    type: string;
    accessibility: 'Public' | 'Private' | 'Protected';
    isAbstract: boolean;
    isSealed: boolean;
    isOverride: boolean;
    isVirtual: boolean;
    isStatic: boolean;
    classId: string;
    localVariables: any[];
    parameters: any[];
  }