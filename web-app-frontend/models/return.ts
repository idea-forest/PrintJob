export interface Return<T> {
  onSuccess: (data: T) => void;
  onError: (error: string) => void;
}
