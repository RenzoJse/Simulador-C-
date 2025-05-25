export interface MethodDTO {
    name: string;
    type: {
      name: string;
      type: string;
    };
    accessibility: 'Public' | 'Private' | 'Protected';
    isAbstract: boolean;
    isSealed: boolean;
    isOverride: boolean;
    isVirtual: boolean;
    classId: string;
    localVariables: any[];
    parameters: any[];
  }