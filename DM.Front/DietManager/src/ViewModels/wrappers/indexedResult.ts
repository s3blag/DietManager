export default interface IndexedResult<T> {
  result: T;
  isLast: boolean;
  index: number;
}
