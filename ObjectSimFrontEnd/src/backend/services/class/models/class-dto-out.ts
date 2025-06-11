export default interface ClassDtoOut {
  id: string;
  name: string;
  isAbstract?: boolean;
  isInterface?: boolean;
  isSealed?: boolean;
  isVirtual?: boolean;
  parent?: string | null;
  attributes: any[];
  methods: any[];
}